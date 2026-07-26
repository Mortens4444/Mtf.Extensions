using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Mtf.Extensions.Tests;

public class EnumExtensionsTests
{
    [Flags]
    private enum TestFlags
    {
        None = 0,

        [Description("A value")]
        A = 1,

        [Description("B value")]
        B = 2
    }

    [Test]
    public void GetEnumAttribute_CombinedFlagValue_DoesNotThrow()
    {
        var combined = TestFlags.A | TestFlags.B;

        Assert.DoesNotThrow(() => combined.GetEnumAttribute<DescriptionAttribute>("Description").ToList());
    }

    [Test]
    public void GetEnumAttribute_CombinedFlagValue_ReturnsEmpty()
    {
        var combined = TestFlags.A | TestFlags.B;

        var result = combined.GetEnumAttribute<DescriptionAttribute>("Description").ToList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetEnumAttribute_SingleDefinedValue_ReturnsDescription()
    {
        var result = TestFlags.A.GetEnumAttribute<DescriptionAttribute>("Description").ToList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("A value"));
    }

    [Test]
    public void GetSingleEnumAttribute_CombinedFlagValue_ReturnsNullInsteadOfThrowing()
    {
        var combined = TestFlags.A | TestFlags.B;

        object result = "unset";
        Assert.DoesNotThrow(() => result = combined.GetSingleEnumAttribute<DescriptionAttribute>("Description"));
        Assert.That(result, Is.Null);
    }
}
