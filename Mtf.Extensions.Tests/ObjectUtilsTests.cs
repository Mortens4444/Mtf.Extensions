namespace Mtf.Extensions.Tests;

public class ObjectUtilsTests
{
    [Test]
    public void Swap_TwoValues_ExchangesThem()
    {
        var a = 1;
        var b = 2;

        ObjectUtils.Swap(ref a, ref b);

        Assert.That(a, Is.EqualTo(2));
        Assert.That(b, Is.EqualTo(1));
    }

    [Test]
    public void Swap_ReferenceTypes_ExchangesThem()
    {
        var a = "first";
        var b = "second";

        ObjectUtils.Swap(ref a, ref b);

        Assert.That(a, Is.EqualTo("second"));
        Assert.That(b, Is.EqualTo("first"));
    }
}
