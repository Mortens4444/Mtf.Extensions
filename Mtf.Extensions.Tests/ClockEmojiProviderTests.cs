using Mtf.Extensions.Services;

namespace Mtf.Extensions.Tests;

public class ClockEmojiProviderTests
{
    private static readonly string[] WholeHourEmojis =
    {
        "🕛", "🕐", "🕑", "🕒", "🕓", "🕔",
        "🕕", "🕖", "🕗", "🕘", "🕙", "🕚"
    };

    private static readonly string[] HalfHourEmojis =
    {
        "🕧", "🕜", "🕝", "🕞", "🕟", "🕠",
        "🕡", "🕢", "🕣", "🕤", "🕥", "🕦"
    };

    [Test]
    public void GetCurrentClockEmoji_ReturnsOneOfTheKnownClockEmojis()
    {
        var result = ClockEmojiProvider.GetCurrentClockEmoji();

        var allKnownEmojis = WholeHourEmojis.Concat(HalfHourEmojis);
        Assert.That(allKnownEmojis, Does.Contain(result));
    }

    [Test]
    public void GetCurrentClockEmoji_DoesNotThrow()
    {
        Ensure.DoesNotThrow(() => ClockEmojiProvider.GetCurrentClockEmoji());
    }
}
