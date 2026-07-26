using Mtf.Extensions.Enums;
using Mtf.Extensions.Models;
using System.Drawing;

namespace Mtf.Extensions.Tests;

public class YUVColorTests
{
    [Test]
    public void Constructor_FromColor_ComputesYUVAndRoundTripsRGB()
    {
        var color = Color.FromArgb(100, 150, 200);

        var yuv = new YUVColor(color);

        Assert.That(yuv.R, Is.EqualTo(100));
        Assert.That(yuv.G, Is.EqualTo(150));
        Assert.That(yuv.B, Is.EqualTo(200));
    }

    [Test]
    public void Constructor_YUVType_RoundTripsThroughRGBWithinRoundingTolerance()
    {
        var original = new YUVColor(Color.FromArgb(100, 150, 200));

        var reconstructed = new YUVColor(original.Y, original.U, original.V, ColorSpaceType.YUV);

        Assert.That(reconstructed.R, Is.EqualTo(original.R).Within(2));
        Assert.That(reconstructed.G, Is.EqualTo(original.G).Within(2));
        Assert.That(reconstructed.B, Is.EqualTo(original.B).Within(2));
    }

    [Test]
    public void Constructor_CdeType_ComputesYuvOffsetsAndClippedRgb()
    {
        var yuv = new YUVColor(0, 0, 0, ColorSpaceType.CDE);

        Assert.That(yuv.Y, Is.EqualTo(16));
        Assert.That(yuv.U, Is.EqualTo(128));
        Assert.That(yuv.V, Is.EqualTo(128));
    }

    [Test]
    public void Constructor_UnsupportedType_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() => new YUVColor(0, 0, 0, (ColorSpaceType)99));
    }

    [Test]
    public void ToColor_ReturnsColorMatchingRGBFields()
    {
        var yuv = new YUVColor(Color.FromArgb(10, 20, 30));

        var color = yuv.ToColor();

        Assert.That(color.R, Is.EqualTo(10));
        Assert.That(color.G, Is.EqualTo(20));
        Assert.That(color.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToColor_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => YUVColor.ConvertToColor(null));
    }

    [Test]
    public void ToString_ContainsRgbAndYuvValues()
    {
        var yuv = new YUVColor(Color.FromArgb(10, 20, 30));

        var text = yuv.ToString();

        Assert.That(text, Does.Contain("RGB"));
        Assert.That(text, Does.Contain("YUV"));
    }

    [TestCase(0, 0, 0)]
    [TestCase(255, 255, 255)]
    [TestCase(255, 0, 0)]
    [TestCase(0, 255, 0)]
    [TestCase(0, 0, 255)]
    public void Constructor_FromColor_NeverThrowsForAnyRgbCombination(int r, int g, int b)
    {
        Assert.DoesNotThrow(() => new YUVColor(Color.FromArgb(r, g, b)));
    }
}
