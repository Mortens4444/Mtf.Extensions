using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Mtf.Extensions.Tests;

public class ObjectExtensionsTests
{
    [Test]
    public void ConvertToBoolean_ConvertsValue()
    {
        object value = "true";
        Assert.That(value.ConvertToBoolean(), Is.True);
    }

    [Test]
    public void ConvertToByte_ConvertsValue()
    {
        object value = 200;
        Assert.That(value.ConvertToByte(), Is.EqualTo((byte)200));
    }

    [Test]
    public void ConvertToChar_ConvertsValue()
    {
        object value = "A";
        Assert.That(value.ConvertToChar(), Is.EqualTo('A'));
    }

    [Test]
    public void ConvertToDateTime_ConvertsValue()
    {
        object value = "2020-01-01";
        Assert.That(value.ConvertToDateTime(), Is.EqualTo(new DateTime(2020, 1, 1)));
    }

    [Test]
    public void ConvertToInt16_ConvertsValue()
    {
        object value = "123";
        Assert.That(value.ConvertToInt16(), Is.EqualTo((short)123));
    }

    [Test]
    public void ConvertToInt32_ConvertsValue()
    {
        object value = "12345";
        Assert.That(value.ConvertToInt32(), Is.EqualTo(12345));
    }

    [Test]
    public void ConvertToUInt16_ConvertsValue()
    {
        object value = "123";
        Assert.That(value.ConvertToUInt16(), Is.EqualTo((ushort)123));
    }

    [Test]
    public void ConvertToUInt32_ConvertsValue()
    {
        object value = "12345";
        Assert.That(value.ConvertToUInt32(), Is.EqualTo((uint)12345));
    }

    [Test]
    public void ConvertToUInt64_ConvertsValue()
    {
        object value = "12345";
        Assert.That(value.ConvertToUInt64(), Is.EqualTo((ulong)12345));
    }

    [Test]
    public void ConvertToInt64_ConvertsValue()
    {
        object value = "12345";
        Assert.That(value.ConvertToInt64(), Is.EqualTo(12345L));
    }

    [Test]
    public void ConvertToString_ConvertsValue()
    {
        object value = 42;
        Assert.That(value.ConvertToString(), Is.EqualTo("42"));
    }

    [Test]
    public void GetDescription_NullValue_ThrowsArgumentNullException()
    {
        object value = null;
        Ensure.Throws<ArgumentNullException>(() => value.GetDescription());
    }

    private enum SampleEnum
    {
        [Description("First value")]
        First,
        Second
    }

    [Test]
    public void GetDescription_EnumWithDescriptionAttribute_ReturnsDescription()
    {
        object value = SampleEnum.First;

        Assert.That(value.GetDescription(), Is.EqualTo("First value"));
    }

    [Test]
    public void GetDescription_EnumWithoutDescriptionAttribute_ReturnsEnumName()
    {
        object value = SampleEnum.Second;

        Assert.That(value.GetDescription(), Is.EqualTo("Second"));
    }

    public class NamedConstant
    {
        [Description("Zero value")]
        public static readonly NamedConstant Zero = new("Zero");

        private readonly string name;

        private NamedConstant(string name) => this.name = name;

        public override string ToString() => name;
    }

    [Test]
    public void GetDescription_NonEnumObjectWithMatchingNamedStaticField_ReturnsFieldDescription()
    {
        object value = NamedConstant.Zero;

        Assert.That(value.GetDescription(), Is.EqualTo("Zero value"));
    }

    [Test]
    public void GetDescription_NoMatchingField_ReturnsToStringValue()
    {
        object value = "no such field on System.String";

        Assert.That(value.GetDescription(), Is.EqualTo("no such field on System.String"));
    }
}
