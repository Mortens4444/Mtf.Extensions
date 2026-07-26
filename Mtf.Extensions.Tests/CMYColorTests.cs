using Mtf.Extensions.Models;
using System.Drawing;

namespace Mtf.Extensions.Tests;

public class CMYColorTests
{
    [Test]
    public void Constructor_Black_ProducesFullInkOnAllChannels()
    {
        var cmy = new CMYColor(Color.FromArgb(0, 0, 0));

        Assert.That(cmy.C, Is.EqualTo(255));
        Assert.That(cmy.M, Is.EqualTo(255));
        Assert.That(cmy.Y, Is.EqualTo(255));
    }

    [Test]
    public void Constructor_White_ProducesNoInk()
    {
        var cmy = new CMYColor(Color.FromArgb(255, 255, 255));

        Assert.That(cmy.C, Is.EqualTo(0));
        Assert.That(cmy.M, Is.EqualTo(0));
        Assert.That(cmy.Y, Is.EqualTo(0));
    }

    [Test]
    public void Constructor_MidGray_ProducesProportionalInk_NotCollapsedToZeroOrOne()
    {
        var cmy = new CMYColor(Color.FromArgb(128, 128, 128));

        Assert.That(cmy.C, Is.EqualTo(127));
        Assert.That(cmy.M, Is.EqualTo(127));
        Assert.That(cmy.Y, Is.EqualTo(127));
    }

    [Test]
    public void ConvertFromCMYToRGB_DoesNotThrowAndComplementsChannels()
    {
        var color = Color.FromArgb(200, 150, 100);

        Color result = default;
        Assert.DoesNotThrow(() => result = color.ConvertFromCMYToRGB());

        Assert.That(result.R, Is.EqualTo(55));
        Assert.That(result.G, Is.EqualTo(105));
        Assert.That(result.B, Is.EqualTo(155));
    }

    [TestCase(0, 0, 0)]
    [TestCase(255, 255, 255)]
    [TestCase(1, 254, 128)]
    [TestCase(77, 3, 199)]
    public void ConvertFromCMYToRGB_VariousInputs_NeverThrows(int r, int g, int b)
    {
        var color = Color.FromArgb(r, g, b);
        Assert.DoesNotThrow(() => color.ConvertFromCMYToRGB());
    }
}
