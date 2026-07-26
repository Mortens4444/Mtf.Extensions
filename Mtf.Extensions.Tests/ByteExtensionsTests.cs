namespace Mtf.Extensions.Tests;

public class ByteExtensionsTests
{
    [TestCase((byte)2, true)]
    [TestCase((byte)3, false)]
    public void IsEven_ReturnsExpectedResult(byte value, bool expected)
    {
        Assert.That(value.IsEven(), Is.EqualTo(expected));
    }

    [TestCase((byte)3, true)]
    [TestCase((byte)2, false)]
    public void IsOdd_ReturnsExpectedResult(byte value, bool expected)
    {
        Assert.That(value.IsOdd(), Is.EqualTo(expected));
    }

    [Test]
    public void LimitMe_ClampsToRange()
    {
        Assert.That(((byte)5).LimitMe(10, 20), Is.EqualTo((byte)10));
        Assert.That(((byte)25).LimitMe(10, 20), Is.EqualTo((byte)20));
        Assert.That(((byte)15).LimitMe(10, 20), Is.EqualTo((byte)15));
    }

    [Test]
    public void LimitMe_MinimumGreaterThanMaximum_SwapsThemFirst()
    {
        Assert.That(((byte)15).LimitMe(20, 10), Is.EqualTo((byte)15));
        Assert.That(((byte)5).LimitMe(20, 10), Is.EqualTo((byte)10));
    }

    [Test]
    public void IsBetweenExclusive_ValueStrictlyInside_ReturnsTrue()
    {
        Assert.That(((byte)5).IsBetweenExclusive(1, 10), Is.True);
    }

    [Test]
    public void IsBetweenExclusive_ValueOnBoundary_ReturnsFalse()
    {
        Assert.That(((byte)1).IsBetweenExclusive(1, 10), Is.False);
        Assert.That(((byte)10).IsBetweenExclusive(1, 10), Is.False);
    }

    [Test]
    public void IsBetweenInclusive_ValueOnBoundary_ReturnsTrue()
    {
        Assert.That(((byte)1).IsBetweenInclusive(1, 10), Is.True);
        Assert.That(((byte)10).IsBetweenInclusive(1, 10), Is.True);
    }

    [Test]
    public void Swap_ExchangesValues()
    {
        byte a = 1;
        byte b = 2;

        ByteExtensions.Swap(ref a, ref b);

        Assert.That(a, Is.EqualTo((byte)2));
        Assert.That(b, Is.EqualTo((byte)1));
    }

    [Test]
    public void GetBitValue_ReturnsPowerOfTwoWhenBitSet()
    {
        Assert.That(((byte)0b0000_0100).GetBitValue(2), Is.EqualTo((byte)4));
        Assert.That(((byte)0b0000_0000).GetBitValue(2), Is.EqualTo((byte)0));
    }

    [Test]
    public void IsBitSet_DetectsSetAndUnsetBits()
    {
        var value = (byte)0b0000_0101;

        Assert.That(value.IsBitSet(0), Is.True);
        Assert.That(value.IsBitSet(1), Is.False);
        Assert.That(value.IsBitSet(2), Is.True);
    }

    [Test]
    public void IsBitPatternSet_AllPatternBitsSet_ReturnsTrue()
    {
        var value = (byte)0b0000_1111;

        Assert.That(value.IsBitPatternSet(0b0000_0011), Is.True);
        Assert.That(value.IsBitPatternSet(0b1000_0000), Is.False);
    }

    [Test]
    public void GetSubBitConbinationValue_ExtractsBitRangeAsValue()
    {
        var value = (byte)0b0010_1100;

        var result = value.GetSubBitConbinationValue(2, 4);

        Assert.That(result, Is.EqualTo((byte)0b0000_1011));
    }
}
