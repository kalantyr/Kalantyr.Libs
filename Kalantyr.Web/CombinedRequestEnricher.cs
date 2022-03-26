using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Kalantyr.Web
{
    public class CombinedRequestEnricher: IRequestEnricher
    {
        private readonly IReadOnlyCollection<IRequestEnricher> _enrichers;

        public CombinedRequestEnricher(IReadOnlyCollection<IRequestEnricher> enrichers)
        {
            _enrichers = enrichers ?? throw new ArgumentNullException(nameof(enrichers));
        }

        public void Enrich(HttpRequestHeaders requestHeaders)
        {
            foreach (var enricher in _enrichers)
                enricher.Enrich(requestHeaders);
        }
    }
}
