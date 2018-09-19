using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class ExchangeInfoDto
    {
        public string Timezone { get; set; }
        public string ServerTime { get; set; }
        public IEnumerable<RateLimitDto> RateLimits { get; set; }
        public IEnumerable<string> ExchangeFilters { get; set; }
        public IEnumerable<SymbolDto> Symbols { get; set; }
    }
}
