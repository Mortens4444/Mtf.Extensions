namespace Mtf.Extensions.Tests;

public class TypeExtensionsMoreTests
{
    public abstract class AbstractBase
    {
    }

    public class ConcreteWithDefaultCtor : AbstractBase
    {
    }

    public class ConcreteWithoutDefaultCtor : AbstractBase
    {
        public ConcreteWithoutDefaultCtor(int value)
        {
        }
    }

    public interface ISampleInterface
    {
    }

    public class InterfaceImplementation : ISampleInterface
    {
    }

    [Test]
    public void InstantiateSubclassesOfAbstractClass_CreatesOnlyConcreteTypesWithDefaultConstructor()
    {
        var allTypes = new[] { typeof(ConcreteWithDefaultCtor), typeof(ConcreteWithoutDefaultCtor), typeof(AbstractBase) };

        var result = typeof(AbstractBase).InstantiateSubclassesOfAbstractClass<AbstractBase>(allTypes);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.InstanceOf<ConcreteWithDefaultCtor>());
    }

    [Test]
    public void InstantiateSubtypes_CreatesInstancesOfAssignableTypes()
    {
        var allTypes = new[] { typeof(InterfaceImplementation), typeof(string) };

        var result = typeof(ISampleInterface).InstantiateSubtypes<ISampleInterface>(allTypes);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.InstanceOf<InterfaceImplementation>());
    }

    [Test]
    public void GetTypesInNamespace_NullSearchedType_ThrowsArgumentNullException()
    {
        Type type = null;

        Assert.Throws<ArgumentNullException>(() => type.GetTypesInNamespace("anything"));
    }

    [Test]
    public void GetTypesInNamespace_ReturnsTypesFromSameAssemblyAndNamespace()
    {
        var result = typeof(TypeExtensionsMoreTests).GetTypesInNamespace(typeof(TypeExtensionsMoreTests).Namespace).ToList();

        Assert.That(result, Does.Contain(typeof(TypeExtensionsMoreTests)));
    }

    [Test]
    public void GetTypeByName_ExistingType_ReturnsIt()
    {
        var result = Mtf.Extensions.TypeExtensions.GetTypeByName(typeof(TypeExtensionsMoreTests).FullName);

        Assert.That(result, Is.EqualTo(typeof(TypeExtensionsMoreTests)));
    }

    [Test]
    public void GetTypeByName_UnknownType_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => Mtf.Extensions.TypeExtensions.GetTypeByName("No.Such.Type.Exists.Anywhere"));
    }

    [TestCase(typeof(int), true)]
    [TestCase(typeof(string), true)]
    [TestCase(typeof(TypeExtensionsMoreTests), false)]
    public void IsPrimitiveOrString_ReturnsExpectedResult(Type type, bool expected)
    {
        Assert.That(type.IsPrimitiveOrString(), Is.EqualTo(expected));
    }

    [Test]
    public void IsPrimitiveOrString_NullType_ReturnsFalse()
    {
        Type type = null;
        Assert.That(type.IsPrimitiveOrString(), Is.False);
    }

    [Test]
    public void IsGenericList_ListType_ReturnsTrueWithElementType()
    {
        var isList = typeof(List<int>).IsGenericList(out var elementType);

        Assert.That(isList, Is.True);
        Assert.That(elementType, Is.EqualTo(typeof(int)));
    }

    [Test]
    public void IsGenericList_SubclassOfList_ReturnsTrueViaBaseTypeWalk()
    {
        var isList = typeof(CustomList).IsGenericList(out var elementType);

        Assert.That(isList, Is.True);
        Assert.That(elementType, Is.EqualTo(typeof(string)));
    }

    [Test]
    public void IsGenericList_NonListType_ReturnsFalse()
    {
        var isList = typeof(int).IsGenericList(out var elementType);

        Assert.That(isList, Is.False);
        Assert.That(elementType, Is.Null);
    }

    public class CustomList : List<string>
    {
    }

    public class WithAttributedProperty
    {
        [System.ComponentModel.Description("prop description")]
        public string Name { get; set; }
    }

    [Test]
    public void GetAttribute_PropertyHasAttribute_ReturnsIt()
    {
        var attribute = typeof(WithAttributedProperty).GetAttribute<System.ComponentModel.DescriptionAttribute>("Name");

        Assert.That(attribute, Is.Not.Null);
        Assert.That(attribute.Description, Is.EqualTo("prop description"));
    }

    [Test]
    public void GetAttribute_NullType_ReturnsNull()
    {
        Type type = null;

        var attribute = type.GetAttribute<System.ComponentModel.DescriptionAttribute>("Name");

        Assert.That(attribute, Is.Null);
    }
}
