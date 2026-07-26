namespace Mtf.Extensions.Tests;

public class IntExtensionsTests
{
    [Test]
    public void ToString_PacksFourBytesIntoCharacters()
    {
        var result = Mtf.Extensions.IntExtensions.ToString(0x41424344u);

        Assert.That(result, Is.EqualTo("ABCD"));
    }

    [TestCase(2, true)]
    [TestCase(3, false)]
    public void IsEven_ReturnsExpectedResult(int value, bool expected)
    {
        Assert.That(value.IsEven(), Is.EqualTo(expected));
    }

    [TestCase(3, true)]
    [TestCase(2, false)]
    public void IsOdd_ReturnsExpectedResult(int value, bool expected)
    {
        Assert.That(value.IsOdd(), Is.EqualTo(expected));
    }

    [Test]
    public void ToBinary_NoMinimumLength_ReturnsUnpaddedBinaryString()
    {
        Assert.That(5.ToBinary(), Is.EqualTo("101"));
    }

    [Test]
    public void ToBinary_WithMinimumLength_PadsWithLeadingZeros()
    {
        Assert.That(5.ToBinary(8), Is.EqualTo("00000101"));
    }

    [Test]
    public void GetBitValue_ReturnsPowerOfTwoWhenBitSet()
    {
        Assert.That(37.GetBitValue(2), Is.EqualTo(4));
    }

    [Test]
    public void IsDivisible_ChecksDivisibilityCorrectly()
    {
        Assert.That(10.IsDivisible(5), Is.True);
        Assert.That(10.IsDivisible(3), Is.False);
    }

    [Test]
    public void IsBetweenExclusive_ReversedBounds_AreSwappedBeforeComparing()
    {
        Assert.That(5.IsBetweenExclusive(10, 1), Is.True);
    }

    [Test]
    public void IsBetweenInclusive_ValueOnBoundary_ReturnsTrue()
    {
        Assert.That(1.IsBetweenInclusive(1, 10), Is.True);
        Assert.That(10.IsBetweenInclusive(1, 10), Is.True);
    }

    [Test]
    public void LimitMe_ReversedBounds_ClampsCorrectlyAfterSwap()
    {
        Assert.That(5.LimitMe(20, 10), Is.EqualTo(10));
        Assert.That(25.LimitMe(20, 10), Is.EqualTo(20));
    }

    [Test]
    public void Swap_ExchangesValues()
    {
        var a = 1;
        var b = 2;

        Mtf.Extensions.IntExtensions.Swap(ref a, ref b);

        Assert.That(a, Is.EqualTo(2));
        Assert.That(b, Is.EqualTo(1));
    }

    [Test]
    public void IsBitSet_DetectsSetAndUnsetBits()
    {
        Assert.That(0b0101.IsBitSet(0), Is.True);
        Assert.That(0b0101.IsBitSet(1), Is.False);
    }

    [Test]
    public void GetSubBitCombinationValue_ExtractsBitRangeAsValue()
    {
        var result = 0b0010_1100.GetSubBitCombinationValue(2, 4);

        Assert.That(result, Is.EqualTo(0b0000_1011));
    }
}
