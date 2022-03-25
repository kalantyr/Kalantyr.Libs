using System;
using System.Net.Http;

namespace Kalantyr.Web
{
    public abstract class HttpClientBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRequestEnricher _requestEnricher;
        private const string ContentType = "application/json";

        protected HttpClientBase(IHttpClientFactory httpClientFactory, IRequestEnricher requestEnricher)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _requestEnricher = requestEnricher;
        }
    }
}
