namespace Mtf.Extensions.Tests;

public class MathExtensionsTests
{
    [Test]
    public void Clamp_IsAccessibleFromTheMtfExtensionsNamespace()
    {
        Assert.That(MathExtensions.Clamp(5m, 0m, 10m), Is.EqualTo(5m));
        Assert.That(MathExtensions.Clamp(-5m, 0m, 10m), Is.EqualTo(0m));
        Assert.That(MathExtensions.Clamp(15m, 0m, 10m), Is.EqualTo(10m));
    }
}
