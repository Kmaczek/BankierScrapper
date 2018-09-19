using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFxBook
{
    public class MoodModel
    {
        public string Symbol { get; set; }

        public float SellPercent { get; set; }

        public float BuyPercent { get; set; }

        public float ShortPriceDistance { get; set; }

        public float LongPriceDistance { get; set; }
    }
}
