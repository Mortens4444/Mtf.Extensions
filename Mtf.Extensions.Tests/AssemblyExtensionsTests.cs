using System.Reflection;

namespace Mtf.Extensions.Tests;

public class AssemblyExtensionsTests
{
    private static class StaticInitProbe
    {
        public static bool Initialized;

        static StaticInitProbe()
        {
            Initialized = true;
        }
    }

    public class SampleClassA
    {
    }

    public class SampleClassB
    {
    }

    [Test]
    public void GetTypesInNamespace_NullAssembly_ThrowsArgumentNullException()
    {
        Assembly assembly = null;

        Ensure.Throws<ArgumentNullException>(() => assembly.GetTypesInNamespace("Mtf.Extensions.Tests"));
    }

    [Test]
    public void GetTypesInNamespace_ReturnsTypesOrderedByName()
    {
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        var types = assembly.GetTypesInNamespace(typeof(AssemblyExtensionsTests).Namespace).ToList();

        Assert.That(types, Does.Contain(typeof(SampleClassA)));
        Assert.That(types, Does.Contain(typeof(SampleClassB)));
        var indexA = types.IndexOf(typeof(SampleClassA));
        var indexB = types.IndexOf(typeof(SampleClassB));
        Assert.That(indexA, Is.LessThan(indexB));
    }

    [Test]
    public void InitializeStaticObjects_RunsStaticConstructorsInNamespace()
    {
        var assembly = typeof(AssemblyExtensionsTests).Assembly;

        assembly.InitializeStaticObjects(typeof(StaticInitProbe).Namespace);

        Assert.That(StaticInitProbe.Initialized, Is.True);
    }
}
