using System.Globalization;
using Data;
using Domain.Enumerators;
using Domain.Models;
using Infra.Utils;
using Worker.Process;
using File = Domain.Models.File;

namespace Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppDbContext _dbContext;

    private const string DadosAbertosCnpjLink = "https://arquivos.receitafederal.gov.br/dados/cnpj/dados_abertos_cnpj/";

    public Worker(
        ILogger<Worker> logger,
        AppDbContext dbContext
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        GetFolderLinks().Wait();
        GetFileLinks().Wait();
        DownloadFiles().Wait();
        ProcessFiles().Wait();
    }

    private async Task GetFolderLinks()
    {
        _logger.LogInformation("Obtaining links");
        var htmlDocument = await HtmlAgilityPackUtil.GetHtmlDocumentAsync(DadosAbertosCnpjLink);
        var trs = htmlDocument.DocumentNode.SelectNodes("//table")
            .Where(node => node.SelectNodes("//a").Any(a => a.Attributes["href"].Value.Contains('-')))
            .SelectMany(node => node.DescendantsAndSelf().Where(n => n.Name.Equals("tr") && n.DescendantsAndSelf().Any(nn => nn.Name.Equals("a") && nn.GetAttributeValue("href", "").Contains('-'))))
            .ToList();
        var folderLinks = trs
            .Select(x => new Folder(
                DadosAbertosCnpjLink + x.Descendants("a").First().GetAttributeValue("href", ""),
                DateTime.ParseExact(x.Descendants("td").First(t => t.InnerText.Contains(':')).InnerText.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)));
        var foldersDb = _dbContext.Folders.ToList();
        foreach (var folderLink in folderLinks)
        {
            var folderDb = foldersDb.FirstOrDefault(x => x.Url == folderLink.Url);
            if (folderDb == null)
            {
                _dbContext.Folders.Add(folderLink);
            }
            else
            {
                if (folderDb.ModificationDate >= folderLink.ModificationDate) continue;
                folderDb.ModificationDate = folderLink.ModificationDate;
                _dbContext.Update(folderDb);
            }
        }

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Links obtained");
    }

    private async Task GetFileLinks()
    {
        _logger.LogInformation("Obtaining file links");
        var folderLink = _dbContext.Folders.OrderByDescending(x => x.ModificationDate).FirstOrDefault();
        if (folderLink == null)
        {
            _logger.LogInformation("No folder links found");
            return;
        }

        var htmlDocument = await HtmlAgilityPackUtil.GetHtmlDocumentAsync(folderLink.Url);
        var trs = htmlDocument.DocumentNode.SelectNodes("//table")
            .Where(node => node.Descendants("a").Any(a => a.GetAttributeValue("href", "").Contains(".zip")))
            .SelectMany(node => node.Descendants().Where(n => n.Name.Equals("tr") && n.Descendants().Any(nn => nn.Name.Equals("a") && nn.GetAttributeValue("href", "").Contains(".zip"))))
            .ToList();
        var fileLinks = trs
            .Select(x => new File(
                folderLink.Url + x.Descendants("td").Skip(1).First().Descendants("a").First().GetAttributeValue("href", ""),
                DateTime.ParseExact(x.Descendants("td").Skip(2).First().InnerText.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                x.Descendants("td").Skip(3).First().InnerText.Trim()))
            .ToList();
        var fileLinksDb = _dbContext.Files.ToList();
        foreach (var fileLink in fileLinks)
        {
            var fileLinkDb = fileLinksDb.FirstOrDefault(x => x.Url == fileLink.Url);
            if (fileLinkDb == null)
            {
                _dbContext.Files.Add(fileLink);
            }
            else
            {
                if (fileLinkDb.ModificationDate >= fileLink.ModificationDate) continue;
                fileLinkDb.ModificationDate = fileLink.ModificationDate;
                _dbContext.Update(fileLinkDb);
            }
        }

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("File links obtained");
    }

    private async Task DownloadFiles()
    {
        _logger.LogInformation("Downloading Files");
        var filesLinks = _dbContext.Files.ToList();
        foreach (var fileLink in filesLinks)
        {
            _logger.LogInformation("Downloading File: {File}", fileLink.Url);
            await DownloaderUtil.DownloadFileAsync(fileLink.Url, $"{AppDomain.CurrentDomain.BaseDirectory}/Files/{fileLink.Url.Split("/").Last()}");
        }
        _logger.LogInformation("Files downloaded");
    }

    private async Task ProcessFiles(){
        var files = _dbContext.Files.ToList();
        var factory = new ProcessFactory(_dbContext);
        Dictionary<FileType, List<File>> filesByType = [];
        var fileTypeNames = Enum.GetNames<FileType>().Order().ToList();
        foreach (var file in files){
            var fileType = fileTypeNames.FirstOrDefault(x => file.Url.Contains(x));
            if (fileType == null) continue;
            if (!filesByType.ContainsKey(Enum.Parse<FileType>(fileType))) 
                filesByType[Enum.Parse<FileType>(fileType)] = [];
            filesByType[Enum.Parse<FileType>(fileType)].Add(file);
        }
        foreach (var fileType in filesByType.OrderBy(x => x.Key)){
            await factory.ProcessFilesAsync(fileType.Value, fileType.Key);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}