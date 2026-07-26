using System.Globalization;
using System.Threading;

namespace Mtf.Extensions.Tests;

public class DateTimeExtensionsTests
{
    private static void WithInvariantCulture(Action action)
    {
        var original = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            action();
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = original;
        }
    }

    [Test]
    public void ToStringInPreferredFormat_MatchesShortDateAndShortTime()
    {
        WithInvariantCulture(() =>
        {
            var date = new DateTime(2012, 11, 15, 14, 2, 35);

            var result = date.ToStringInPreferredFormat();

            Assert.That(result, Is.EqualTo($"{date.ToShortDateString()} {date.ToShortTimeString()}"));
        });
    }

    [Test]
    public void ToStringInPreferredFormatWithSeconds_MatchesShortDateAndLongTime_SecondsNotDuplicated()
    {
        WithInvariantCulture(() =>
        {
            var date = new DateTime(2012, 11, 15, 14, 2, 35);

            var result = date.ToStringInPreferredFormatWithSeconds();

            Assert.That(result, Is.EqualTo($"{date.ToShortDateString()} {date.ToLongTimeString()}"));
        });
    }
}
