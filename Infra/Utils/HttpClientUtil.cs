using System;

namespace Infra.Utils;

public static class HttpClientUtil
{
  private static readonly HttpClient _httpClient = new();

  public static async Task<string> GetStringAsync(string url, CancellationToken cancellationToken = default) => await _httpClient.GetStringAsync(url, cancellationToken);
  public static async Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken = default) => await _httpClient.GetStreamAsync(url, cancellationToken);
  public static async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken = default) => await _httpClient.GetAsync(url, cancellationToken);
  public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message,HttpCompletionOption option, CancellationToken cancellationToken = default) => await _httpClient.SendAsync(message,option, cancellationToken);
  public static async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default) => await _httpClient.PostAsync(url, content, cancellationToken);
  public static async Task<HttpResponseMessage> PutAsync(string url, HttpContent content, CancellationToken cancellationToken = default) => await _httpClient.PutAsync(url, content, cancellationToken);
  public static async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default) => await _httpClient.DeleteAsync(url, cancellationToken);
}
