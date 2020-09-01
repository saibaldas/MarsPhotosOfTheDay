using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsPhotosOfTheDay.Services.Net
{
    public class HttpRequester : IHttpRequester, IDisposable
    {
        private bool _disposed;

        private readonly IUriBuilder _uriBuilder;
        private readonly HttpClient _httpClient;

        public HttpRequester(IUriBuilder uriBuilder, HttpClient httpClient = null)
        {
            _uriBuilder = uriBuilder;
            _httpClient = httpClient ?? GetDefaultHttpClient();
        }

        private HttpClient GetDefaultHttpClient()
            => new HttpClient();

        public async Task<HttpResponseMessage> SendHttpRequestAsync()
        {
            ThrowExceptionIfDisposed();

            var uri = _uriBuilder.GetUri();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            }
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime)
        {
            ThrowExceptionIfDisposed();

            var uri = _uriBuilder.GetUri(dateTime);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            }
        }

        private static IUriBuilder GetUriBuilder(string apiKey)
                => new UriBuilder(apiKey);

        private void ThrowExceptionIfDisposed()
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }
        }

        public void Dispose()
        {
            if (_disposed) { return; }
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
