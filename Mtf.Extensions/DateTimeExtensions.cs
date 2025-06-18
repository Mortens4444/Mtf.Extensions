using System;

namespace Mtf.Extensions
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime SqlMinValue = new DateTime(1753, 1, 1, 12, 0, 0);

        /// <summary>
        /// Preferred to string format
        /// </summary>
        /// <param name="date">The date to represent in string format</param>
        /// <returns>String representation of the date. For example: 2012.11.15 14:02</returns>
        public static string ToStringInPreferredFormat(this DateTime date)
        {
            return $"{date.ToShortDateString()} {date.ToLongTimeString()}";
        }
        
        public static string ToFriendlyString(this DateTime dateTime)
        {
            return $"{dateTime:yyyy.MM.dd HH:mm:ss}";
        }

        /// <summary>
        /// Preferred to string format
        /// </summary>
        /// <param name="date">The date to represent in string format</param>
        /// <returns>String representation of the date. For example: 2012.11.15 14:02:35</returns>
        public static string ToStringInPreferredFormatWithSeconds(this DateTime date)
        {
            return $"{date.ToShortDateString()} {date.ToLongTimeString()}:{date.Second:D2}";
        }
    }
}
