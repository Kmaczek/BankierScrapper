using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    public class ServerTimeDto
    {
        [JsonProperty("serverTime")]
        public long ServerTimestamp { get; set; }

        [JsonIgnore]
        public DateTime ServerTime
        {
            get
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(ServerTimestamp).UtcDateTime;
            }
        }
    }
}
