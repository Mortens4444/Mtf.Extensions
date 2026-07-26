namespace Mtf.Extensions.Tests;

public class ByteArrayExtensionsTests
{
    [Test]
    public void EqualInPercent_BothNull_Returns100_ConsistentWithBothEmptyCase()
    {
        var result = ByteArrayExtensions.EqualInPercent(null, null);

        Assert.That((byte)result, Is.EqualTo((byte)100));
    }

    [Test]
    public void EqualInPercent_BothEmpty_Returns100()
    {
        var result = ByteArrayExtensions.EqualInPercent(Array.Empty<byte>(), Array.Empty<byte>());

        Assert.That((byte)result, Is.EqualTo((byte)100));
    }

    [Test]
    public void EqualInPercent_OneNull_ReturnsZero()
    {
        var result = ByteArrayExtensions.EqualInPercent(null, new byte[] { 1, 2, 3 });

        Assert.That((byte)result, Is.EqualTo((byte)0));
    }

    [Test]
    public void EqualInPercent_IdenticalArrays_Returns100()
    {
        var result = ByteArrayExtensions.EqualInPercent(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 });

        Assert.That((byte)result, Is.EqualTo((byte)100));
    }

    [Test]
    public void IsEqual_BothNull_ReturnsTrue_ConsistentWithEqualInPercent()
    {
        Assert.That(ByteArrayExtensions.IsEqual(null, null), Is.True);
    }
}
