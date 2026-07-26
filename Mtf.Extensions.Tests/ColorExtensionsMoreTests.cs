using Mtf.Extensions.Enums;
using System.Drawing;

namespace Mtf.Extensions.Tests;

public class ColorExtensionsMoreTests
{
    [Test]
    public void GetBT601Value_ColorOverload_MatchesComponentOverload()
    {
        var color = Color.FromArgb(100, 150, 200);

        var viaColor = color.GetBT601Value();
        var viaComponents = ColorExtensions.GetBT601Value(100 / 255.0, 150 / 255.0, 200 / 255.0);

        Assert.That(viaColor, Is.EqualTo(viaComponents).Within(0.0001));
    }

    [Test]
    public void GetBT709Value_ColorOverload_MatchesComponentOverload()
    {
        var color = Color.FromArgb(100, 150, 200);

        var viaColor = color.GetBT709Value();
        var viaComponents = ColorExtensions.GetBT709Value(100 / 255.0, 150 / 255.0, 200 / 255.0);

        Assert.That(viaColor, Is.EqualTo(viaComponents).Within(0.0001));
    }

    [Test]
    public void GetNormalizedValue_ColorOverload_MatchesComponentsOverload()
    {
        var color = Color.FromArgb(10, 20, 30);

        Assert.That(ColorExtensions.GetNormalizedValue(ColorComponent.Red, color),
            Is.EqualTo(ColorExtensions.GetNormalizedValue(ColorComponent.Red, 10, 20, 30)));
    }

    [Test]
    public void GetNormalizedValue_SumsToOneAcrossAllComponents()
    {
        var r = ColorExtensions.GetNormalizedValue(ColorComponent.Red, 10, 20, 30);
        var g = ColorExtensions.GetNormalizedValue(ColorComponent.Green, 10, 20, 30);
        var b = ColorExtensions.GetNormalizedValue(ColorComponent.Blue, 10, 20, 30);

        Assert.That(r + g + b, Is.EqualTo(1.0).Within(0.0001));
    }

    [Test]
    public void GetNormalizedValue_UnsupportedComponent_ThrowsNotSupportedException()
    {
        Ensure.Throws<NotSupportedException>(() => ColorExtensions.GetNormalizedValue((ColorComponent)99, 10, 20, 30));
    }

    [Test]
    public void GetNonLinearGammaCorrectedValue_DividesBy255()
    {
        Assert.That(ColorExtensions.GetNonLinearGammaCorrectedValue(255), Is.EqualTo(1.0));
        Assert.That(ColorExtensions.GetNonLinearGammaCorrectedValue(0), Is.EqualTo(0.0));
    }

    [Test]
    public void GetComponentValue_MultipliesBy255AndRounds()
    {
        Assert.That(ColorExtensions.GetComponentValue(1.0), Is.EqualTo(255));
        Assert.That(ColorExtensions.GetComponentValue(0.0), Is.EqualTo(0));
    }

    [Test]
    public void InverseColor_InvertsAllChannelsKeepingAlpha()
    {
        var color = Color.FromArgb(200, 10, 20, 30);

        var result = color.InverseColor();

        Assert.That(result.A, Is.EqualTo(200));
        Assert.That(result.R, Is.EqualTo(245));
        Assert.That(result.G, Is.EqualTo(235));
        Assert.That(result.B, Is.EqualTo(225));
    }

    [Test]
    public void IsColorBetweenColors_ValueInsideRange_ReturnsTrue()
    {
        var value = Color.FromArgb(255, 50, 50, 50);
        var from = Color.FromArgb(255, 0, 0, 0);
        var to = Color.FromArgb(255, 100, 100, 100);

        Assert.That(value.IsColorBetweenColors(from, to), Is.True);
    }

    [Test]
    public void IsColorBetweenColors_ValueOutsideRange_ReturnsFalse()
    {
        var value = Color.FromArgb(255, 150, 50, 50);
        var from = Color.FromArgb(255, 0, 0, 0);
        var to = Color.FromArgb(255, 100, 100, 100);

        Assert.That(value.IsColorBetweenColors(from, to), Is.False);
    }

    [Test]
    public void ConvertToBlackOrWhite_DarkColor_ReturnsBlack()
    {
        Assert.That(Color.FromArgb(0, 0, 0).ConvertToBlackOrWhite(), Is.EqualTo(Color.Black));
    }

    [Test]
    public void ConvertToBlackOrWhite_BrightColor_ReturnsWhite()
    {
        Assert.That(Color.FromArgb(255, 255, 255).ConvertToBlackOrWhite(), Is.EqualTo(Color.White));
    }

    private static readonly Func<Color, Color>[] GrayscaleConversions =
    {
        c => c.ConvertToSimpleAvarageGrayscale(),
        c => c.ConvertToWeightedAvarageGrayscale(),
        c => c.ConvertToGrayscale1(),
        c => c.ConvertToGrayscale2(),
        c => c.ConvertToBT601Grayscale(),
        c => c.ConvertToGrayscale4(),
        c => c.ConvertToBT609Grayscale(),
        c => c.ConvertToBT709Grayscale(),
        c => c.ConvertToRMYGrayscale()
    };

    [Test]
    public void GrayscaleConversions_ProduceEqualRGBChannels()
    {
        var color = Color.FromArgb(120, 80, 200);

        foreach (var convert in GrayscaleConversions)
        {
            var result = convert(color);
            Assert.That(result.R, Is.EqualTo(result.G));
            Assert.That(result.G, Is.EqualTo(result.B));
        }
    }

    [Test]
    public void ConvertToRedscale_KeepsOnlyRedChannel()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToRedscale();
        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(0));
        Assert.That(result.B, Is.EqualTo(0));
    }

    [Test]
    public void ConvertToGreenscale_KeepsOnlyGreenChannel()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToGreenscale();
        Assert.That(result.R, Is.EqualTo(0));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(0));
    }

    [Test]
    public void ConvertToBluescale_KeepsOnlyBlueChannel()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToBluescale();
        Assert.That(result.R, Is.EqualTo(0));
        Assert.That(result.G, Is.EqualTo(0));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToRedGreenscale_DropsBlueOnly()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToRedGreenscale();
        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(0));
    }

    [Test]
    public void ConvertToGreenBluescale_DropsRedOnly()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToGreenBluescale();
        Assert.That(result.R, Is.EqualTo(0));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToRedBluescale_DropsGreenOnly()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToRedBluescale();
        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(0));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToYUVColor_ReturnsYUVColorForSameRGB()
    {
        var color = Color.FromArgb(10, 20, 30);

        var yuv = color.ConvertToYUVColor();

        Assert.That(yuv.R, Is.EqualTo(10));
        Assert.That(yuv.G, Is.EqualTo(20));
        Assert.That(yuv.B, Is.EqualTo(30));
    }

    private static readonly Action<Color>[] YuvAndCmyScaleConversions =
    {
        c => c.ConvertToYUVYScale(),
        c => c.ConvertToYUVUScale(),
        c => c.ConvertToYUVVScale(),
        c => c.ConvertToYUVYUScale(),
        c => c.ConvertToYUVUVScale(),
        c => c.ConvertToYUVYVScale(),
        c => c.ConvertToCMYCScale(),
        c => c.ConvertToCMYMScale(),
        c => c.ConvertToCMYYScale(),
        c => c.ConvertToCMYCMScale(),
        c => c.ConvertToCMYMYScale(),
        c => c.ConvertToCMYCYScale()
    };

    [TestCase(0, 0, 0)]
    [TestCase(255, 255, 255)]
    [TestCase(10, 200, 90)]
    public void YuvAndCmyScaleConversions_NeverThrow(int r, int g, int b)
    {
        var color = Color.FromArgb(r, g, b);

        foreach (var convert in YuvAndCmyScaleConversions)
        {
            Ensure.DoesNotThrow(() => convert(color));
        }
    }

    [Test]
    public void ConvertToInverse_InvertsAllChannels()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToInverse();
        Assert.That(result.R, Is.EqualTo(245));
        Assert.That(result.G, Is.EqualTo(235));
        Assert.That(result.B, Is.EqualTo(225));
    }

    [Test]
    public void ConvertToInverseRed_OnlyInvertsRed()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToInverseRed();
        Assert.That(result.R, Is.EqualTo(245));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToInverseGreen_OnlyInvertsGreen()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToInverseGreen();
        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(235));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToInverseBlue_OnlyInvertsBlue()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToInverseBlue();
        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(225));
    }

    [Test]
    public void ConvertToInverseRedBlue_InvertsRedAndBlueOnly()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToInverseRedBlue();
        Assert.That(result.R, Is.EqualTo(245));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(225));
    }

    [Test]
    public void ConvertToRBG_SwapsGreenAndBlue()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToRBG();
        Assert.That(result.R, Is.EqualTo(10));
        Assert.That(result.G, Is.EqualTo(30));
        Assert.That(result.B, Is.EqualTo(20));
    }

    [Test]
    public void ConvertToBGR_ReversesAllChannels()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToBGR();
        Assert.That(result.R, Is.EqualTo(30));
        Assert.That(result.G, Is.EqualTo(20));
        Assert.That(result.B, Is.EqualTo(10));
    }

    [Test]
    public void ConvertToGRB_RotatesChannels()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToGRB();
        Assert.That(result.R, Is.EqualTo(20));
        Assert.That(result.G, Is.EqualTo(10));
        Assert.That(result.B, Is.EqualTo(30));
    }

    [Test]
    public void ConvertToGBR_RotatesChannels()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToGBR();
        Assert.That(result.R, Is.EqualTo(20));
        Assert.That(result.G, Is.EqualTo(30));
        Assert.That(result.B, Is.EqualTo(10));
    }

    [Test]
    public void ConvertToBRG_RotatesChannels()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToBRG();
        Assert.That(result.R, Is.EqualTo(30));
        Assert.That(result.G, Is.EqualTo(10));
        Assert.That(result.B, Is.EqualTo(20));
    }

    [Test]
    public void ConvertFromYUVToRGB_DoesNotThrow()
    {
        Ensure.DoesNotThrow(() => Color.FromArgb(10, 20, 30).ConvertFromYUVToRGB());
    }

    [Test]
    public void ConvertToExp_ClampsResultWithinByteRange()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToExp();
        Assert.That(result.R, Is.InRange(0, 255));
        Assert.That(result.G, Is.InRange(0, 255));
        Assert.That(result.B, Is.InRange(0, 255));
    }

    [Test]
    public void ConvertToPow_ClampsResultWithinByteRange()
    {
        var result = Color.FromArgb(10, 20, 30).ConvertToPow();
        Assert.That(result.R, Is.InRange(0, 255));
        Assert.That(result.G, Is.InRange(0, 255));
        Assert.That(result.B, Is.InRange(0, 255));
    }

    [Test]
    public void GetRandomColor_ReturnsOpaqueColor()
    {
        var color = ColorExtensions.GetRandomColor();

        Assert.That(color.A, Is.EqualTo(255));
    }

    [TestCaseSource(nameof(AllTransformMethods))]
    public void TransformColor_EveryMethod_DoesNotThrow(ColorTransformMethod method)
    {
        var color = Color.FromArgb(10, 20, 30);

        Ensure.DoesNotThrow(() => color.TransformColor(method));
    }

    private static IEnumerable<ColorTransformMethod> AllTransformMethods()
    {
        return Enum.GetValues(typeof(ColorTransformMethod)).Cast<ColorTransformMethod>();
    }

    [Test]
    public void TransformColor_UnsupportedMethod_ThrowsNotSupportedException()
    {
        Ensure.Throws<NotSupportedException>(() => Color.Red.TransformColor((ColorTransformMethod)255));
    }

    [Test]
    public void TransformColor_Original_ReturnsSameColor()
    {
        var color = Color.FromArgb(10, 20, 30);

        var result = color.TransformColor(ColorTransformMethod.Original);

        Assert.That(result, Is.EqualTo(color));
    }
}
