using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mtf.Extensions.Tests;

public class BrushExtensionsTests
{
    [Test]
    public void ToColor_SolidBrush_ReturnsItsColor()
    {
        using var brush = new SolidBrush(Color.FromArgb(10, 20, 30));

        var color = ((Brush)brush).ToColor();

        Assert.That(color.R, Is.EqualTo(10));
        Assert.That(color.G, Is.EqualTo(20));
        Assert.That(color.B, Is.EqualTo(30));
    }

    [Test]
    public void ToColor_LinearGradientBrush_ReturnsFirstColor()
    {
        using var brush = new LinearGradientBrush(new Rectangle(0, 0, 10, 10), Color.Red, Color.Blue, 45f);

        var color = ((Brush)brush).ToColor();

        Assert.That(color.ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
    }

    [Test]
    public void ToColor_HatchBrush_ReturnsForegroundColor()
    {
        using var brush = new HatchBrush(HatchStyle.Cross, Color.Green, Color.White);

        var color = ((Brush)brush).ToColor();

        Assert.That(color.ToArgb(), Is.EqualTo(Color.Green.ToArgb()));
    }

    [Test]
    public void ToColor_UnsupportedBrushType_ThrowsInvalidOperationException()
    {
        using var brush = new TextureBrush(new Bitmap(1, 1));

        Ensure.Throws<InvalidOperationException>(() => ((Brush)brush).ToColor());
    }
}
