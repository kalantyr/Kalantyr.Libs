using System.Net.Http.Headers;

namespace Kalantyr.Web.Impl
{
    public class RequestEnricher : IRequestEnricher
    {
        public TokenRequestEnricher TokenEnricher { get; } = new();

        public AppKeyRequestEnricher AppKeyEnricher { get; } = new();

        public void Enrich(HttpRequestHeaders requestHeaders)
        {
            TokenEnricher.Enrich(requestHeaders);
            AppKeyEnricher.Enrich(requestHeaders);
        }
    }
}
