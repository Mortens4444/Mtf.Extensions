using System;

namespace Mtf.Extensions.Services
{
    public static class ClockEmojiProvider
    {
        private static readonly string[] WholeHours =
        {
            "🕛","🕐","🕑","🕒","🕓","🕔",
            "🕕","🕖","🕗","🕘","🕙","🕚"
        };

        private static readonly string[] HalfHours =
        {
            "🕧","🕜","🕝","🕞","🕟","🕠",
            "🕡","🕢","🕣","🕤","🕥","🕦"
        };

        public static string GetCurrentClockEmoji()
        {
            var now = DateTime.Now;

            var hour = now.Hour % 12;
            var minute = now.Minute;

            if (minute >= 45)
            {
                hour = (hour + 1) % 12;
                return WholeHours[hour];
            }

            if (minute >= 15)
            {
                return HalfHours[hour];
            }

            return WholeHours[hour];
        }
    }
}
