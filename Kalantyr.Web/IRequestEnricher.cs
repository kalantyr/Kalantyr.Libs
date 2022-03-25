using System.Net.Http.Headers;

namespace Kalantyr.Web
{
    public interface IRequestEnricher
    {
        void Enrich(HttpRequestHeaders requestHeaders);
    }
}
