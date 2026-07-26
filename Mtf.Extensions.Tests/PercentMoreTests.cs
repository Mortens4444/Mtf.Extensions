using Mtf.Extensions.Models;

namespace Mtf.Extensions.Tests;

public class PercentMoreTests
{
    [Test]
    public void Constructor_ValueOver100_ThrowsByDefault()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Percent(150));
    }

    [Test]
    public void Constructor_ValueOver100WithThrowExceptionFalse_ClampsTo100()
    {
        var percent = new Percent(150, false);

        Assert.That(percent.Value, Is.EqualTo((byte)100));
    }

    [Test]
    public void ToString_FormatsAsValueWithPercentSign()
    {
        Percent percent = 42;

        Assert.That(percent.ToString(), Is.EqualTo("42%"));
    }

    [Test]
    public void ToProbability_ConvertsToFraction()
    {
        Percent percent = 50;

        Assert.That(percent.ToProbability(), Is.EqualTo(0.5f));
    }

    [Test]
    public void EqualityOperators_SameValue_AreEqual()
    {
        Percent a = 50;
        Percent b = 50;

        Assert.That(a == b, Is.True);
        Assert.That(a != b, Is.False);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals((object)b), Is.True);
    }

    [Test]
    public void EqualityOperators_DifferentValue_AreNotEqual()
    {
        Percent a = 50;
        Percent b = 60;

        Assert.That(a == b, Is.False);
        Assert.That(a != b, Is.True);
    }

    [Test]
    public void Equals_NonPercentObject_ReturnsFalse()
    {
        Percent a = 50;

        Assert.That(a.Equals("50"), Is.False);
    }

    [Test]
    public void GetHashCode_SameValue_ProducesSameHash()
    {
        Percent a = 50;
        Percent b = 50;

        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void ToByte_ReturnsUnderlyingValue()
    {
        Percent percent = 42;

        Assert.That(Percent.ToByte(percent), Is.EqualTo((byte)42));
    }

    [Test]
    public void ToPercent_CreatesFromByte()
    {
        var percent = Percent.ToPercent(30);

        Assert.That(percent.Value, Is.EqualTo((byte)30));
    }

    [Test]
    public void ImplicitByteConversion_ReturnsValue()
    {
        Percent percent = 42;

        byte value = percent;

        Assert.That(value, Is.EqualTo((byte)42));
    }
}
