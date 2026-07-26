using Mtf.Extensions.Services;

namespace Mtf.Extensions.Tests;

public class RandomProviderMoreTests
{
    [Test]
    public void GetSecureRandomByte_StaysWithinRange()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomProvider.GetSecureRandomByte(10, 20);
            Assert.That(value, Is.InRange((byte)10, (byte)19));
        }
    }

    [Test]
    public void GetSecureRandomShort_StaysWithinRange()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomProvider.GetSecureRandomShort(-100, 100);
            Assert.That(value, Is.InRange((short)-100, (short)99));
        }
    }

    [Test]
    public void GetSecureRandomUInt_StaysWithinRange()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomProvider.GetSecureRandomUInt(10u, 20u);
            Assert.That(value, Is.InRange(10u, 19u));
        }
    }

    [Test]
    public void GetSecureRandomUInt64WithRange_StaysWithinRange()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomProvider.GetSecureRandomUInt64(10ul, 20ul);
            Assert.That(value, Is.InRange(10ul, 19ul));
        }
    }

    [Test]
    public void GetSecureRandom_MinEqualsMax_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => RandomProvider.GetSecureRandomInt(5, 5));
    }

    [Test]
    public void GetSecureRandom_MinGreaterThanMax_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => RandomProvider.GetSecureRandomInt(10, 5));
    }

    [Test]
    public void GetSecureRandomUInt64_NoArgs_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => RandomProvider.GetSecureRandomUInt64());
    }

    [Test]
    public void GetSecureRandomDouble_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => RandomProvider.GetSecureRandomDouble());
    }

    [Test]
    public void GetSecureRandomProbability_StaysWithinZeroToOne()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomProvider.GetSecureRandomProbability();
            Assert.That(value, Is.InRange(0.0, 1.0));
        }
    }
}
