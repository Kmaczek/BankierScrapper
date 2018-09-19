using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class TradeDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public long Time { get; set; }
        public bool IsBuyerMaker { get; set; }
        public bool IsBestMatch { get; set; }

    }
}
