using Utils;

namespace Mtf.Windows.Forms.Extensions.Tests;

public class ComboBoxItemTests
{
    [Test]
    public void ToString_NullObject_ReturnsEmptyStringInsteadOfThrowing()
    {
        var item = new ComboBoxItem(null);

        Assert.That(item.ToString(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ToString_NonNullObject_ReturnsObjectToString()
    {
        var item = new ComboBoxItem(42);

        Assert.That(item.ToString(), Is.EqualTo("42"));
    }
}
