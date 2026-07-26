namespace Mtf.Extensions.Tests;

public class UInt16ExtensionsTests
{
    [Test]
    public void ToHexString_PadsToFourUppercaseHexDigits()
    {
        ushort value = 0xAB;

        Assert.That(value.ToHexString(), Is.EqualTo("00AB"));
    }

    [Test]
    public void IsBitSet_DetectsSetAndUnsetBits()
    {
        ushort value = 0b0101;

        Assert.That(value.IsBitSet(0), Is.True);
        Assert.That(value.IsBitSet(1), Is.False);
        Assert.That(value.IsBitSet(2), Is.True);
    }

    [Test]
    public void GetSubBitConbinationValue_ExtractsBitRangeAsValue()
    {
        ushort value = 0b0010_1100;

        var result = value.GetSubBitConbinationValue(2, 4);

        Assert.That(result, Is.EqualTo((ushort)0b0000_1011));
    }
}
