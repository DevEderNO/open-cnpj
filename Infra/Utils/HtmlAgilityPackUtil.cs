using HtmlAgilityPack;

namespace Infra.Utils;

public static class HtmlAgilityPackUtil
{
    public static async Task<HtmlDocument> GetHtmlDocumentAsync(string url, CancellationToken cancellationToken = default)
    {
        var htmlDocument = new HtmlDocument();
        var response = await HttpClientUtil.GetAsync(url, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        htmlDocument.LoadHtml(content);
        return htmlDocument;
    }
}