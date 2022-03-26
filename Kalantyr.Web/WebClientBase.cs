using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kalantyr.Web
{
    public abstract class HttpClientBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRequestEnricher _requestEnricher;

        protected IRequestEnricher RequestEnricher => _requestEnricher;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private const string ContentType = "application/json";

        protected HttpClientBase(IHttpClientFactory httpClientFactory, IRequestEnricher requestEnricher)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _requestEnricher = requestEnricher;
        }
        protected async Task<T> Get<T>(string path, CancellationToken cancellationToken)
        {
            return await SendAsync<T>(HttpMethod.Get, path, null, cancellationToken);
        }

        protected async Task<T> Post<T>(string path, string body, CancellationToken cancellationToken)
        {
            return await SendAsync<T>(HttpMethod.Post, path, body, cancellationToken);
        }

        protected async Task<T> SendAsync<T>(HttpMethod method, string path, string body, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(GetType().Name);

            var uri = string.Join("/", httpClient.BaseAddress.AbsoluteUri.TrimEnd('/'), path.TrimStart('/'));
            using (var requestMessage = new HttpRequestMessage(method, new Uri(uri, UriKind.Absolute)))
            {
                _requestEnricher?.Enrich(requestMessage.Headers);

                if (Debugger.IsAttached)
                {
                    Debug.WriteLine(method + " " + httpClient.BaseAddress + path);
                    if (method != HttpMethod.Get)
                        if (!string.IsNullOrEmpty(body))
                            Debug.WriteLine(body);
                }

                if (method != HttpMethod.Get)
                    if (!string.IsNullOrEmpty(body))
                        requestMessage.Content = new StringContent(body, Encoding.UTF8, ContentType);

                requestMessage.Headers.Add("Accept", ContentType);

                using (var result = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken))
                    return await FromResponse<T>(result);
            }
        }

        internal async Task<T> FromResponse<T>(HttpResponseMessage result)
        {
            var response = await result.Content.ReadAsStringAsync();

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:

                    if (string.IsNullOrWhiteSpace(response))
                        return default;

                    if (typeof(T) == typeof(string))
                        return (T)(object)response;

                    return JsonSerializer.Deserialize<T>(response, JsonSerializerOptions);

                case HttpStatusCode.NoContent:
                    return default;

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new Exception("Доступ запрещён");

                default:
                    if (string.IsNullOrWhiteSpace(response))
                        throw new Exception($"HTTP {(int)result.StatusCode} {result.StatusCode}");
                    else
                        throw new Exception($"HTTP {(int)result.StatusCode} {result.StatusCode}", new Exception(response));
            }
        }
    }
}
