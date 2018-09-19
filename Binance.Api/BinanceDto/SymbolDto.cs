using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class SymbolDto
    {
        public string Symbol { get; set; }
        public string Status { get; set; }
        public string BaseAsset { get; set; }
        public int BaseAssetPrecission { get; set; }
        public string QuoteAsset { get; set; }
        public int QuotePrecission { get; set; }
        public IEnumerable<string> OrderTypes { get; set; }
        public bool IcebergAllowed { get; set; }
        public IEnumerable<FilterDto> Filters { get; set; }
    }
}
