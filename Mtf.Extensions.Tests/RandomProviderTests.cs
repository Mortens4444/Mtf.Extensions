using Mtf.Extensions.Services;

namespace Mtf.Extensions.Tests;

public class RandomProviderTests
{
    [Test]
    public void GetSecureRandomInt_NegativeRange_DoesNotThrowAndStaysInRange()
    {
        for (var i = 0; i < 200; i++)
        {
            int value = 0;
            Ensure.DoesNotThrow(() => value = RandomProvider.GetSecureRandomInt(-10, 10));
            Assert.That(value, Is.InRange(-10, 9));
        }
    }

    [Test]
    public void GetSecureRandomInt64_NegativeRange_DoesNotThrow()
    {
        Ensure.DoesNotThrow(() => RandomProvider.GetSecureRandomInt64(-1000L, 1000L));
    }

    [Test]
    public void GetSecureRandomInt_PositiveRange_StillWorks()
    {
        for (var i = 0; i < 50; i++)
        {
            var value = RandomProvider.GetSecureRandomInt(5, 15);
            Assert.That(value, Is.InRange(5, 14));
        }
    }
}
