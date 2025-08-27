# ProcessFactory - Padrão Factory para Processamento de Arquivos

## Visão Geral

O `ProcessFactory` implementa o padrão Factory para processar diferentes tipos de arquivos baseados no enum `FileType`. Este padrão permite processar arquivos de forma polimórfica, onde cada tipo de arquivo tem sua própria lógica de processamento.

## Estrutura

### Interface IFileProcessor
```csharp
public interface IFileProcessor
{
    FileType FileType { get; }
    Task ProcessAsync(FileModel file);
}
```

### Classe Base BaseFileProcessor
Fornece implementação base com validação comum e template method pattern.

### Processadores Específicos
- `CnaesProcessor` - Processa arquivos de CNAEs
- `EmpresasProcessor` - Processa arquivos de Empresas
- `EstabelecimentosProcessor` - Processa arquivos de Estabelecimentos
- `MotivosProcessor` - Processa arquivos de Motivos
- `MunicipiosProcessor` - Processa arquivos de Municípios
- `NaturezasProcessor` - Processa arquivos de Naturezas
- `PaisesProcessor` - Processa arquivos de Países
- `QualificacoesProcessor` - Processa arquivos de Qualificações
- `SimplesProcessor` - Processa arquivos do Simples Nacional
- `SociosProcessor` - Processa arquivos de Sócios

## Como Usar

### Uso Básico
```csharp
var factory = new ProcessFactory();
var file = new File("https://exemplo.com/arquivo.zip", DateTime.Now, new VoFileSize(1024));

// Processar arquivo específico
await factory.ProcessFileAsync(file, FileType.Cnaes);
```

### Verificar Suporte
```csharp
if (factory.HasProcessor(FileType.Empresas))
{
    await factory.ProcessFileAsync(file, FileType.Empresas);
}
```

### Obter Processador Específico
```csharp
var processor = factory.GetProcessor(FileType.Estabelecimentos);
await processor.ProcessAsync(file);
```

### Listar Tipos Suportados
```csharp
var supportedTypes = factory.GetSupportedFileTypes();
foreach (var fileType in supportedTypes)
{
    Console.WriteLine(fileType);
}
```

## Vantagens do Padrão

1. **Extensibilidade**: Fácil adicionar novos tipos de processamento
2. **Manutenibilidade**: Cada processador tem responsabilidade única
3. **Testabilidade**: Processadores podem ser testados isoladamente
4. **Flexibilidade**: Permite trocar implementações sem afetar o código cliente
5. **Reutilização**: Lógica comum na classe base

## Extensões Futuras

Para adicionar um novo tipo de processamento:

1. Adicionar o novo valor ao enum `FileType`
2. Criar nova classe que herda de `BaseFileProcessor`
3. Implementar o método `ProcessFileContentAsync`
4. Adicionar o processador ao dicionário no construtor do `ProcessFactory`

## Exemplo de Extensão

```csharp
public class NovoProcessor : BaseFileProcessor
{
    public override FileType FileType => FileType.NovoTipo;
    
    protected override async Task ProcessFileContentAsync(FileModel file)
    {
        // Implementação específica
        await Task.CompletedTask;
    }
}
```
