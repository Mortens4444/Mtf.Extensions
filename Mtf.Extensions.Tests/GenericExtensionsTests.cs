namespace Mtf.Extensions.Tests;

public class GenericExtensionsTests
{
    // Note: int/byte have their own more-specific non-generic LimitMe overloads
    // (IntExtensions.LimitMe, ByteExtensions.LimitMe) that win overload resolution
    // over this generic one, so we exercise it with a type that has no competing
    // overload (double only has LimitMeWithRound, a different name) plus an explicit
    // static call to unambiguously hit GenericExtensions.LimitMe<T> itself.

    [Test]
    public void LimitMe_ValueBelowMinimum_ReturnsMinimum()
    {
        Assert.That(5.0.LimitMe(10.0, 20.0), Is.EqualTo(10.0));
    }

    [Test]
    public void LimitMe_ValueAboveMaximum_ReturnsMaximum()
    {
        Assert.That(25.0.LimitMe(10.0, 20.0), Is.EqualTo(20.0));
    }

    [Test]
    public void LimitMe_ValueWithinRange_ReturnsValueUnchanged()
    {
        Assert.That(15.0.LimitMe(10.0, 20.0), Is.EqualTo(15.0));
    }

    [Test]
    public void LimitMe_ExplicitGenericCall_ClampsCorrectly()
    {
        Assert.That(GenericExtensions.LimitMe(5, 10, 20), Is.EqualTo(10));
        Assert.That(GenericExtensions.LimitMe(25, 10, 20), Is.EqualTo(20));
        Assert.That(GenericExtensions.LimitMe(15, 10, 20), Is.EqualTo(15));
    }
}
