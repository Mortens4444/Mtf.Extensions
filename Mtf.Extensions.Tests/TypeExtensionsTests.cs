namespace Mtf.Extensions.Tests;

public class TypeExtensionsTests
{
    [Test]
    public void IsArray_ActualArrayTypes_ReturnsTrue()
    {
        Assert.That(typeof(int[]).IsArray(), Is.True);
        Assert.That(typeof(string[]).IsArray(), Is.True);
        Assert.That(typeof(object[]).IsArray(), Is.True);
    }

    [Test]
    public void IsArray_NonArrayTypes_ReturnsFalse()
    {
        Assert.That(typeof(int).IsArray(), Is.False);
        Assert.That(typeof(string).IsArray(), Is.False);
        Assert.That(typeof(Array).IsArray(), Is.False);
    }

    [Test]
    public void IsArray_Null_ReturnsFalse()
    {
        Type type = null;
        Assert.That(type.IsArray(), Is.False);
    }
}
