using System.Text;
using Data;
using Domain.Enumerators;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Readers;
using FileModel = Domain.Models.File;
using File = System.IO.File;
using Infra.ValueObjects;
using System.Globalization;
using Infra.Utils;

namespace Worker.Process;

public interface IFileProcessor
{
    FileType FileType { get; }
    Task ProcessAsync(FileModel file);
}

public abstract class BaseFileProcessor : IFileProcessor
{
    public abstract FileType FileType { get; }

    public virtual async Task ProcessAsync(FileModel file)
    {
        // Implementação base que pode ser sobrescrita
        await ValidateFileAsync(file);
        await ProcessFileContentAsync(file);
    }

    protected void ResetFileProgress(FileModel file)
    {
        file.Progress = 0;
        file.LineNumber = 0;
    }

    private static async Task ValidateFileAsync(FileModel file)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (string.IsNullOrEmpty(file.Url))
            throw new ArgumentException("URL do arquivo não pode ser nula ou vazia");

        await Task.CompletedTask;
    }

    protected abstract Task ProcessFileContentAsync(FileModel file);
}

public class CnaesProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Cnaes;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        using var reader = ReaderFactory.Open(File.OpenRead($"{AppDomain.CurrentDomain.BaseDirectory}/Files/{file.FileName}"), new ReaderOptions());

        var cnaes = new List<DCnae>();
        long currentLineNumber = file.LineNumber;

        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory) continue;
            Console.WriteLine($"Extracting {reader.Entry.Key}");
            var totalSize = reader.Entry.Size;
            await using var entryStream = reader.OpenEntryStream();
            var progressStream = new ProgressStreamUtil(entryStream);
            using var sr = new StreamReader(progressStream, Encoding.Latin1);
            
            // Pular linhas já processadas
            for (long i = 0; i < currentLineNumber && !sr.EndOfStream; i++)
            {
                await sr.ReadLineAsync();
            }
            
            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (line == null) continue;
                
                currentLineNumber++;
                var cnae = line[1..^1].Split("\";\"");
                //Console.WriteLine($"CNAE: {cnae[0]} - {cnae[1]}");
                cnaes.Add(new DCnae(
                    cnae[0],
                    cnae[1],
                    file.ModificationDate
                ));
                
                // Atualizar progresso baseado em bytes e número da linha
                file.Progress = (float)(progressStream.BytesRead / (decimal)totalSize);
                file.LineNumber = currentLineNumber;
                
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Progresso: {file.Progress * 100:F2}% - Linha: {currentLineNumber}");
                
                // Salvar progresso a cada 1000 linhas ou quando o buffer estiver cheio
                if (currentLineNumber % 1000 == 0 || cnaes.Count >= 10000)
                {
                    dbContext.Files.Update(file);
                    await dbContext.SaveChangesAsync();
                }
            }
            dbContext.Files.Update(file);
            await dbContext.SaveChangesAsync();
        }

        var cnaesDb = dbContext.Cnaes.ToList();
        var updatedCnaes = cnaesDb.Where(x => cnaes.Select(y => y.Cnae.Code).Contains(x.Cnae.Code) && x.ModificationDate < file.ModificationDate).ToList();
        var newCnaes = cnaes.Where(x => !cnaesDb.Select(y => y.Cnae.Code).Contains(x.Cnae.Code)).ToList();

        if (newCnaes.Count != 0)
        {
            await dbContext.Cnaes.BulkInsertAsync(newCnaes);
        }

        if (updatedCnaes.Count != 0)
        {
            foreach (var updatedCnae in updatedCnaes)
            {
                var cnae = cnaes.First(x => x.Cnae.Code == updatedCnae.Cnae.Code);
                updatedCnae.Descricao = cnae.Descricao;
                updatedCnae.ModificationDate = cnae.ModificationDate;
            }

            await dbContext.Cnaes.BulkUpdateAsync(updatedCnaes);
        }

        Console.WriteLine("CNAEs saved");
        await Task.CompletedTask;
    }
}

public class EmpresasProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Empresas;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        using var reader = ReaderFactory.Open(File.OpenRead($"{AppDomain.CurrentDomain.BaseDirectory}/Files/{file.FileName}"), new ReaderOptions());
        long currentLineNumber = file.LineNumber;
        
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory) continue;
            Console.WriteLine($"Extracting {reader.Entry.Key}");
            var totalSize = reader.Entry.Size;
            await using var entryStream = reader.OpenEntryStream();
            var empresas = new List<Empresa>();
            var progressStream = new ProgressStreamUtil(entryStream);
            using var sr = new StreamReader(progressStream, Encoding.Latin1);
            
            // Pular linhas já processadas
            for (long i = 0; i < currentLineNumber && !sr.EndOfStream; i++)
            {
                await sr.ReadLineAsync();
            }
            
            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (line == null) continue;
                
                currentLineNumber++;
                var data = line[1..^1].Split("\";\"").Select(x => x.Trim()).ToList();
                _ = decimal.TryParse(data[4], out var capitalSocial);

                empresas.Add(new Empresa(
                    data[0],
                    data[1],
                    data[2],
                    data[3],
                    capitalSocial,
                    data[5],
                    data[6],
                    file.ModificationDate
                ));

                file.Progress = (float)(progressStream.BytesRead / (decimal)totalSize);
                file.LineNumber = currentLineNumber;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Progresso: {file.Progress * 100:F2}% - Linha: {currentLineNumber}");

                if (empresas.Count < 100000 && !sr.EndOfStream) continue;
                
                await dbContext.Empresas.WhereNotExistsBulkInsertAsync(empresas);
                empresas.Clear();
                dbContext.Files.Update(file);
                await dbContext.SaveChangesAsync();
            }
        }

        Console.WriteLine("Empresas extracted");


        await Task.CompletedTask;
    }
}

public class EstabelecimentosProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Estabelecimentos;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        var errosPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Files/EstabelecimentosErros.txt";
        using var reader = ReaderFactory.Open(File.OpenRead($"{AppDomain.CurrentDomain.BaseDirectory}/Files/{file.FileName}"), new ReaderOptions());
        var erros = new StringBuilder(await File.ReadAllTextAsync(errosPath));
        var cnaes = dbContext.Cnaes.ToList();
        var cnaesSecudariosPorCnpj = new Dictionary<VoCnpj, List<DCnae>>();
        var currentLineNumber = file.LineNumber;
        
        while (reader.MoveToNextEntry())
        {
            var estabelecimentos = new List<Estabelecimento>();
            if (reader.Entry.IsDirectory) continue;
            Console.WriteLine($"Extracting {reader.Entry.Key}");
            var totalSize = reader.Entry.Size;
            await using var entryStream = reader.OpenEntryStream();
            var progressStream = new ProgressStreamUtil(entryStream);
            using var sr = new StreamReader(progressStream, Encoding.Latin1);
            
            // Pular linhas já processadas
            for (long i = 0; i < currentLineNumber && !sr.EndOfStream; i++)
            {
                await sr.ReadLineAsync();
            }
            
            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                currentLineNumber++;
                var data = line[1..^1].Split("\";\"").Select(x => x.Trim()).ToList();
                if (data.Count < 30)
                {
                    erros.AppendLine(line);
                    await File.WriteAllBytesAsync(errosPath, Encoding.UTF8.GetBytes(erros.ToString()));
                    continue;
                }
                cnaesSecudariosPorCnpj.Add($"{data[0]}{data[1]}{data[2]}", cnaes.Where(x => data[12].Split(",").Select(y => y.Trim()).Contains(x.Cnae.Code)).ToList());
                DateTime.TryParseExact(data[6], "yyyyMMdd", null, DateTimeStyles.None, out var dataSituacaoCadastral);
                DateTime.TryParseExact(data[10], "yyyyMMdd", null, DateTimeStyles.None, out var dataInicioAtividade);
                DateTime.TryParseExact(data[29], "yyyyMMdd", null, DateTimeStyles.None, out var dataSituacaoEspecial);
                var estabelecimento = new Estabelecimento(
                    $"{data[0]}{data[1]}{data[2]}",
                    data[0],
                    data[1],
                    data[2],
                    data[3],
                    data[4],
                    data[5],
                    dataSituacaoCadastral,
                    data[7],
                    data[8],
                    data[9],
                    dataInicioAtividade,
                    data[11],
                    data[13],
                    data[14],
                    data[15],
                    data[16],
                    data[17],
                    data[18],
                    data[19],
                    data[20],
                    !string.IsNullOrWhiteSpace(data[21]) && data[21].Length > 2 ? data[21][^2..]: data[21],
                    data[22],
                    !string.IsNullOrWhiteSpace(data[23]) && data[23].Length > 2 ? data[23][^2..] : data[23], 
                    data[24],
                    !string.IsNullOrWhiteSpace(data[25]) && data[25].Length > 2 ? data[25][^2..] : data[25],
                    data[26],
                    new VoEmail(data[27]),
                    data[28],
                    dataSituacaoEspecial == DateTime.MinValue ? null : dataSituacaoEspecial,
                    file.ModificationDate
                );

                estabelecimentos.Add(estabelecimento);

                file.Progress = (float)(progressStream.BytesRead / (decimal)totalSize);
                file.LineNumber = currentLineNumber;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Progresso: {file.Progress * 100:F2}% - Linha: {currentLineNumber}   ");

                if (estabelecimentos.Count < 10000 && !sr.EndOfStream) continue;
                await dbContext.Estabelecimentos.WhereNotExistsBulkInsertAsync(estabelecimentos);
                dbContext.Files.Update(file);
                await dbContext.SaveChangesAsync();

                var cnaesSecundariosParaInserir = new List<EstabelecimentoCnaeSecundario>();

                foreach (var estab in estabelecimentos)
                {
                    var cnaesSecundarios = cnaesSecudariosPorCnpj.GetValueOrDefault(estab.Cnpj, []);
                    if (cnaesSecundarios.Count > 0)
                    {
                        cnaesSecundariosParaInserir.AddRange(
                            cnaesSecundarios.Select(x => new EstabelecimentoCnaeSecundario
                            {
                                CnaeSecundario = x,
                                Estabelecimento = estab,
                            }));
                    }
                }

                if (cnaesSecundariosParaInserir.Count > 0)
                {
                    await dbContext.EstabelecimentoCnaesSecundarios.WhereNotExistsBulkInsertAsync(cnaesSecundariosParaInserir);
                }

                estabelecimentos.Clear();
                cnaesSecudariosPorCnpj.Clear();
            }
        }

        Console.WriteLine("Estabelecimentos extracted");


        await Task.CompletedTask;
    }
}

public class MotivosProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Motivos;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos de Motivos
        Console.WriteLine($"Processando arquivo de Motivos: {file.Url}");
        await Task.CompletedTask;
    }
}

public class MunicipiosProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Municipios;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos de Municípios
        Console.WriteLine($"Processando arquivo de Municípios: {file.Url}");
        await Task.CompletedTask;
    }
}

public class NaturezasProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Naturezas;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos de Naturezas
        Console.WriteLine($"Processando arquivo de Naturezas: {file.Url}");
        await Task.CompletedTask;
    }
}

public class PaisesProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Paises;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos de Países
        Console.WriteLine($"Processando arquivo de Países: {file.Url}");
        await Task.CompletedTask;
    }
}

public class QualificacoesProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Qualificacoes;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos de Qualificações
        Console.WriteLine($"Processando arquivo de Qualificações: {file.Url}");
        await Task.CompletedTask;
    }
}

public class SimplesProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Simples;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos do Simples Nacional
        Console.WriteLine($"Processando arquivo do Simples Nacional: {file.Url}");
        await Task.CompletedTask;
    }
}

public class SociosProcessor(AppDbContext dbContext) : BaseFileProcessor
{
    public override FileType FileType => FileType.Socios;

    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica para processar arquivos de Sócios
        Console.WriteLine($"Processando arquivo de Sócios: {file.Url}");
        await Task.CompletedTask;
    }
}

public class ProcessFactory
{
    private readonly Dictionary<FileType, IFileProcessor> _processors;

    public ProcessFactory(AppDbContext dbContext)
    {
        _processors = new Dictionary<FileType, IFileProcessor>
        {
            { FileType.Cnaes, new CnaesProcessor(dbContext) },
            { FileType.Empresas, new EmpresasProcessor(dbContext) },
            { FileType.Estabelecimentos, new EstabelecimentosProcessor(dbContext) },
            { FileType.Motivos, new MotivosProcessor(dbContext) },
            { FileType.Municipios, new MunicipiosProcessor(dbContext) },
            { FileType.Naturezas, new NaturezasProcessor(dbContext) },
            { FileType.Paises, new PaisesProcessor(dbContext) },
            { FileType.Qualificacoes, new QualificacoesProcessor(dbContext) },
            { FileType.Simples, new SimplesProcessor(dbContext) },
            { FileType.Socios, new SociosProcessor(dbContext) }
        };
    }

    public IFileProcessor GetProcessor(FileType fileType)
    {
        if (_processors.TryGetValue(fileType, out var processor))
        {
            return processor;
        }

        throw new ArgumentException($"Processador não encontrado para o tipo de arquivo: {fileType}");
    }

    public async Task ProcessFileAsync(
        FileModel file,
        FileType fileType
    )
    {
        var processor = GetProcessor(fileType);
        await processor.ProcessAsync(file);
    }

    public async Task ProcessFilesAsync(
        List<FileModel> files,
        FileType fileType
    )
    {
        var processor = GetProcessor(fileType);
        foreach (var file in files)
        {
            await processor.ProcessAsync(file);
        }
    }

    public bool HasProcessor(FileType fileType)
    {
        return _processors.ContainsKey(fileType);
    }

    public IEnumerable<FileType> GetSupportedFileTypes()
    {
        return _processors.Keys;
    }

    public bool IsFileFullyProcessed(FileModel file)
    {
        return file.Progress >= 1.0f;
    }

    public async Task ResetFileProgressAsync(FileModel file, AppDbContext dbContext)
    {
        file.Progress = 0;
        file.LineNumber = 0;
        await dbContext.SaveChangesAsync();
    }
}