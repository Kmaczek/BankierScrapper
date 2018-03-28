using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Common
{
    public static class DateExtensions
    {
        private static string bankerDateFormat = "yyyy-MM-dd";

        public static string ToBankerString(this DateTime dateTime)
        {
            return dateTime.ToString(bankerDateFormat);
        }
    }
}
