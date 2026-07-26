using System.Drawing;

namespace Mtf.Extensions.Tests;

public class ColorExtensionsTests
{
    [Test]
    public void ConvertToInverseGreenBlue_OnlyInvertsGreenAndBlue()
    {
        var color = Color.FromArgb(10, 20, 30);
        var result = color.ConvertToInverseGreenBlue();

        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(235));
        Assert.That(result.B, Is.EqualTo(225));
    }

    [Test]
    public void ConvertToInverseRedGreen_OnlyInvertsRedAndGreen()
    {
        var color = Color.FromArgb(10, 20, 30);
        var result = color.ConvertToInverseRedGreen();

        Assert.That(result.R, Is.EqualTo(245));
        Assert.That(result.G, Is.EqualTo(235));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ColorFromHSV_PureRed_ReturnsExactRedWithoutThrowing()
    {
        Color result = default;
        Ensure.DoesNotThrow(() => result = ColorExtensions.ColorFromHSV(0, 1, 1));
        Assert.That(result.R, Is.EqualTo(255));
        Assert.That(result.G, Is.EqualTo(0));
        Assert.That(result.B, Is.EqualTo(0));
    }

    [TestCase(0, 1, 1)]
    [TestCase(60, 1, 1)]
    [TestCase(120, 1, 1)]
    [TestCase(180, 1, 1)]
    [TestCase(240, 1, 1)]
    [TestCase(300, 1, 1)]
    [TestCase(360, 1, 1)]
    [TestCase(90, 0.5, 0.75)]
    [TestCase(0, 0, 0)]
    [TestCase(0, 0, 1)]
    public void ColorFromHSV_VariousInputs_NeverThrows(double hue, double saturation, double value)
    {
        Ensure.DoesNotThrow(() => ColorExtensions.ColorFromHSV(hue, saturation, value));
    }
}
