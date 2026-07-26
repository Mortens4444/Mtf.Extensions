namespace Mtf.Extensions.Tests;

public class DoubleExtensionsTests
{
    [Test]
    public void LimitMeWithRound_ClampsAndRoundsToNearestInt()
    {
        Assert.That((5.6).LimitMeWithRound(10, 20), Is.EqualTo(10));
        Assert.That((25.4).LimitMeWithRound(10, 20), Is.EqualTo(20));
        Assert.That((15.6).LimitMeWithRound(10, 20), Is.EqualTo(16));
    }

    [Test]
    public void LimitMeWithRound_ReversedBounds_AreSwappedBeforeClamping()
    {
        Assert.That((5.0).LimitMeWithRound(20, 10), Is.EqualTo(10));
    }

    [Test]
    public void Swap_ExchangesValues()
    {
        var a = 1.5;
        var b = 2.5;

        Mtf.Extensions.DoubleExtensions.Swap(ref a, ref b);

        Assert.That(a, Is.EqualTo(2.5));
        Assert.That(b, Is.EqualTo(1.5));
    }

    [Test]
    public void RoundToInt_RoundsToNearestInteger()
    {
        Assert.That((2.5).RoundToInt(), Is.EqualTo(2)); // banker's rounding
        Assert.That((2.6).RoundToInt(), Is.EqualTo(3));
    }

    [Test]
    public void TruncateToInt_DropsFractionalPart()
    {
        Assert.That((2.9).TruncateToInt(), Is.EqualTo(2));
        Assert.That((-2.9).TruncateToInt(), Is.EqualTo(-2));
    }
}
