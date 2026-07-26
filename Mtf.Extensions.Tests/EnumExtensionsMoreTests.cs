using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Mtf.Extensions.Tests;

public class EnumExtensionsMoreTests
{
    private enum PlainEnum
    {
        [DescriptionAttribute("First item")]
        First,
        Second
    }

    [Flags]
    private enum FlagsEnum
    {
        None = 0,

        [DescriptionAttribute("Flag A")]
        A = 1,

        [DescriptionAttribute("Flag B")]
        B = 2,

        [DescriptionAttribute("Flag C")]
        C = 4
    }

    private enum DisabledAwareEnum
    {
        Enabled1,
        Enabled2,

        [Mtf.Extensions.Attributes.Disabled]
        Disabled1
    }

    [Test]
    public void GetDescription_Enum_HasAttribute_ReturnsDescription()
    {
        Assert.That(PlainEnum.First.GetDescription(), Is.EqualTo("First item"));
    }

    [Test]
    public void GetDescription_Enum_NoAttribute_ReturnsEnumName()
    {
        Assert.That(PlainEnum.Second.GetDescription(), Is.EqualTo("Second"));
    }

    [Test]
    public void GetDescriptionGeneric_HasAttribute_ReturnsDescription()
    {
        Assert.That(PlainEnum.First.GetDescription<PlainEnum>(), Is.EqualTo("First item"));
    }

    [Test]
    public void GetDescriptionGeneric_NoAttribute_ReturnsEnumName()
    {
        Assert.That(PlainEnum.Second.GetDescription<PlainEnum>(), Is.EqualTo("Second"));
    }

    [Test]
    public void GetFromDescription_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => EnumExtensions.GetFromDescription<PlainEnum>(null));
    }

    [Test]
    public void GetFromDescription_MatchingEnumName_ReturnsEnumValue()
    {
        var result = EnumExtensions.GetFromDescription<PlainEnum>("First");

        Assert.That(result, Is.EqualTo(PlainEnum.First));
    }

    [Test]
    public void GetFromDescription_StripsParenthesesSuffix()
    {
        var result = EnumExtensions.GetFromDescription<PlainEnum>("Second (extra info)");

        Assert.That(result, Is.EqualTo(PlainEnum.Second));
    }

    [Test]
    public void GetFromDescription_UnknownName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EnumExtensions.GetFromDescription<PlainEnum>("NoSuchValue"));
    }

    [Test]
    public void GetValueFromDescription_MatchingDescription_ReturnsEnumValue()
    {
        var result = EnumExtensions.GetValueFromDescription<PlainEnum>("First item");

        Assert.That(result, Is.EqualTo(PlainEnum.First));
    }

    [Test]
    public void GetValueFromDescription_UnknownDescription_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => EnumExtensions.GetValueFromDescription<PlainEnum>("no such description"));
    }

    [Test]
    public void GetComplexDescription_ZeroValue_ReturnsItsOwnDescription()
    {
        var result = FlagsEnum.None.GetComplexDescription();

        Assert.That(result, Is.EqualTo("None"));
    }

    [Test]
    public void GetComplexDescription_SingleFlag_ReturnsItsDescription()
    {
        var result = FlagsEnum.A.GetComplexDescription();

        Assert.That(result, Is.EqualTo("Flag A"));
    }

    [Test]
    public void GetComplexDescription_CombinedFlags_JoinsDescriptions()
    {
        var combined = FlagsEnum.A | FlagsEnum.C;

        var result = combined.GetComplexDescription();

        Assert.That(result, Is.EqualTo("Flag A, Flag C"));
    }

    [Test]
    public void GetIndividualValues_ReturnsOnlyPowerOfTwoValues()
    {
        var result = EnumExtensions.GetIndividualValues(typeof(FlagsEnum));

        Assert.That(result.Cast<FlagsEnum>(), Is.EquivalentTo(new[] { FlagsEnum.A, FlagsEnum.B, FlagsEnum.C }));
    }

    [Test]
    public void HasAnyFlag_NullValue_ThrowsArgumentNullException()
    {
        Enum value = null;

        Assert.Throws<ArgumentNullException>(() => value.HasAnyFlag(FlagsEnum.A));
    }

    [Test]
    public void HasAnyFlag_ValueHasOneOfTheFlags_ReturnsTrue()
    {
        var value = FlagsEnum.A | FlagsEnum.B;

        Assert.That(value.HasAnyFlag(FlagsEnum.C, FlagsEnum.B), Is.True);
    }

    [Test]
    public void HasAnyFlag_ValueHasNoneOfTheFlags_ReturnsFalse()
    {
        var value = FlagsEnum.A;

        Assert.That(value.HasAnyFlag(FlagsEnum.B, FlagsEnum.C), Is.False);
    }

    [Test]
    public void GetEnabledValues_ExcludesValuesMarkedDisabled()
    {
        var result = EnumExtensions.GetEnabledValues<DisabledAwareEnum>().ToList();

        Assert.That(result, Does.Contain(DisabledAwareEnum.Enabled1));
        Assert.That(result, Does.Contain(DisabledAwareEnum.Enabled2));
        Assert.That(result, Does.Not.Contain(DisabledAwareEnum.Disabled1));
    }
}
