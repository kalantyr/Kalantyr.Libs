using System.Net.Http.Headers;

namespace Kalantyr.Web.Impl
{
    public class AppKeyRequestEnricher : IRequestEnricher
    {
        public string AppKey { get; set; }

        public void Enrich(HttpRequestHeaders requestHeaders)
        {
            if (string.IsNullOrEmpty(AppKey))
                return;

            requestHeaders.Add("App-Key", AppKey);
        }
    }
}
