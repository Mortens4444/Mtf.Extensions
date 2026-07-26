using System.Drawing;

namespace Mtf.Extensions.Tests;

public class RectangleExtensionsTests
{
    [Test]
    public void GetMiddle_ReturnsCenterPoint()
    {
        var rectangle = new Rectangle(10, 20, 100, 50);

        var middle = rectangle.GetMiddle();

        Assert.That(middle.X, Is.EqualTo(60));
        Assert.That(middle.Y, Is.EqualTo(45));
    }

    [Test]
    public void GetMiddleX_ReturnsHorizontalCenter()
    {
        var rectangle = new Rectangle(10, 20, 100, 50);

        Assert.That(rectangle.GetMiddleX(), Is.EqualTo(60));
    }

    [Test]
    public void GetMiddleY_ReturnsVerticalCenter()
    {
        var rectangle = new Rectangle(10, 20, 100, 50);

        Assert.That(rectangle.GetMiddleY(), Is.EqualTo(45));
    }
}
