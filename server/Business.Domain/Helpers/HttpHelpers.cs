using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Business.Domain.Helpers
{
    public static class HttpHelpers
    {
        private static readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy =
            Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(r => r.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3));

        public static async Task<HttpResponseMessage> SendRequestRaw(string path, HttpMethod method, AuthenticationHeaderValue authentication = null, string content = null, string queryString = null)
        {
            using (var client = new HttpClient())
            {
                if (authentication != null)
                    client.DefaultRequestHeaders.Authorization = authentication;

                var builder = new UriBuilder(path);

                if (queryString != null)
                    builder.Query += queryString;

                var httpRequest = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(builder.ToString())
                };

                if (content != null)
                    httpRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");

                var resultado = await _retryPolicy.ExecuteAsync(() => client.SendAsync(httpRequest));

                return resultado;
            }
        }
    }
}
