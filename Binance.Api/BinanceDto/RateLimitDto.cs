using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class RateLimitDto
    {
        public string RateLimitType { get; set; }
        public string Interval { get; set; }
        public int Limit { get; set; }
    }
}
