namespace Mtf.Extensions.Tests;

public class Int64ExtensionsTests
{
    [Test]
    public void ToTimeSpan_ScalesCentisecondRemainderToMillisecondsCorrectly()
    {
        var timeSpan = 12345L.ToTimeSpan();

        Assert.That(timeSpan.Milliseconds, Is.EqualTo(450));
        Assert.That(timeSpan.Seconds, Is.EqualTo(3));
        Assert.That(timeSpan.Minutes, Is.EqualTo(2));
        Assert.That(timeSpan.Hours, Is.EqualTo(0));
        Assert.That(timeSpan.Days, Is.EqualTo(0));
    }

    [Test]
    public void ToTimeSpan_ZeroRemainder_ProducesZeroMilliseconds()
    {
        var timeSpan = 500L.ToTimeSpan();

        Assert.That(timeSpan.Milliseconds, Is.EqualTo(0));
        Assert.That(timeSpan.Seconds, Is.EqualTo(5));
    }
}
