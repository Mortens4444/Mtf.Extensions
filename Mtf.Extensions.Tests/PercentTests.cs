using Mtf.Extensions.Models;

namespace Mtf.Extensions.Tests;

public class PercentTests
{
    [Test]
    public void ImplicitConversion_ValueOutsideByteRange_ThrowsInsteadOfSilentlyWrapping()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Percent p = 300;
        });
    }

    [Test]
    public void ImplicitConversion_ValueOverHundredButWithinByteRange_StillThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Percent p = 150;
        });
    }

    [Test]
    public void ImplicitConversion_ValidValue_Works()
    {
        Percent p = 50;
        Assert.That((byte)p, Is.EqualTo((byte)50));
    }
}
