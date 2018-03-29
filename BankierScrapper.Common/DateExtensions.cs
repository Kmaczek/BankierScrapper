using System;
using System.Globalization;

namespace BankierScrapper.Common
{
    public static class DateExtensions
    {
        private static string bankerDateFormat = "yyyy-MM-dd";

        public static string ToBankerString(this DateTime dateTime)
        {
            return dateTime.ToString(bankerDateFormat);
        }

        public static DateTime ToBankerDate(this string dateString)
        {
            return DateTime.ParseExact(dateString, bankerDateFormat, CultureInfo.CurrentCulture);
        }
    }
}
