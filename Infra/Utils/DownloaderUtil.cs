using static System.Console;

namespace Infra.Utils;

public static class DownloaderUtil
{
    public static async Task DownloadFileAsync(
        string url,
        string destinationPath
    )
    {
        WriteLine($"Starting download from {url}");

        // Se já existe arquivo parcial, pega o tamanho
        long existingLength = 0;
        if (File.Exists(destinationPath))
            existingLength = new FileInfo(destinationPath).Length;

        // Verifica tamanho total do arquivo
        var headRequest = new HttpRequestMessage(HttpMethod.Head, url);
        var headResponse = await HttpClientUtil.SendAsync(headRequest, HttpCompletionOption.ResponseHeadersRead);
        headResponse.EnsureSuccessStatusCode();
        var totalLength = headResponse.Content.Headers.ContentLength ?? -1;

        if (existingLength == totalLength)
        {
            WriteLine("File already fully downloaded.");
            return;
        }

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (existingLength > 0)
        {
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(existingLength, null);
            WriteLine($"Resuming download from byte {existingLength}");
        }

        using var response = await HttpClientUtil.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        if(!Directory.Exists(destinationPath)) Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? string.Empty);
        
        await using var fs = new FileStream(destinationPath, FileMode.Append, FileAccess.Write, FileShare.None, 8192, true);
        await using var stream = await response.Content.ReadAsStreamAsync();

        var buffer = new byte[8192];
        int bytesRead;
        var totalRead = existingLength;
        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            await fs.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalRead += bytesRead;
            if (totalLength <= 0) continue;
            var percent = (double)totalRead / totalLength * 100;
            SetCursorPosition(0, CursorTop);
            Write($"Progresso: {percent:F2}%   ");
        }

        WriteLine($"Download completed: {destinationPath}");
    }
}