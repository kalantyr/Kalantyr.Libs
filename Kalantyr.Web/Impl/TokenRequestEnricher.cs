using System.Net.Http.Headers;

namespace Kalantyr.Web.Impl
{
    public class TokenRequestEnricher : IRequestEnricher
    {
        public string Token { get; set; }

        public void Enrich(HttpRequestHeaders requestHeaders)
        {
            if (string.IsNullOrEmpty(Token))
                return;

            requestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }
    }
}
