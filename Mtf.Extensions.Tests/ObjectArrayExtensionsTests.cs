namespace Mtf.Extensions.Tests;

public class ObjectArrayExtensionsTests
{
    [Test]
    public void ToArrayString_EmptyRange_ReturnsEmptyStringInsteadOfThrowing()
    {
        var elements = new object[] { 1, 2, 3 };

        Assert.That(elements.ToArrayString(3, 3), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ToArrayString_StartIndexGreaterThanEndIndex_ReturnsEmptyString()
    {
        var elements = new object[] { 1, 2, 3 };

        Assert.That(elements.ToArrayString(2, 1), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ToArrayString_NormalRange_ReturnsJoinedString()
    {
        var elements = new object[] { 1, 2, 3 };

        Assert.That(elements.ToArrayString(0, 3, ','), Is.EqualTo("1,2,3"));
    }
}
