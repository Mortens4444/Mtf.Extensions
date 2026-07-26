namespace Mtf.Extensions.Tests;

public class ReflectionExtensionsTests
{
    public interface ISampleService
    {
    }

    public class SampleServiceA : ISampleService
    {
    }

    public class SampleServiceB : ISampleService
    {
    }

    public abstract class AbstractSampleService : ISampleService
    {
    }

    [Test]
    public void CreateInstancesFromNamespace_StringOverload_EmptyNamespace_ReturnsEmptyList()
    {
        var result = string.Empty.CreateInstancesFromNamespace<ISampleService>();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void CreateInstancesFromNamespace_StringOverload_FindsConcreteImplementations()
    {
        var result = typeof(ReflectionExtensionsTests).Namespace.CreateInstancesFromNamespace<ISampleService>();

        Assert.That(result.Select(r => r.GetType()), Does.Contain(typeof(SampleServiceA)));
        Assert.That(result.Select(r => r.GetType()), Does.Contain(typeof(SampleServiceB)));
        Assert.That(result.Select(r => r.GetType()), Does.Not.Contain(typeof(AbstractSampleService)));
    }

    [Test]
    public void CreateInstancesFromNamespace_StringOverload_ExcludeType_OmitsMatchingInstances()
    {
        var result = typeof(ReflectionExtensionsTests).Namespace.CreateInstancesFromNamespace<ISampleService>(typeof(SampleServiceA));

        Assert.That(result.Select(r => r.GetType()), Does.Not.Contain(typeof(SampleServiceA)));
        Assert.That(result.Select(r => r.GetType()), Does.Contain(typeof(SampleServiceB)));
    }

    [Test]
    public void CreateInstancesFromNamespace_AssemblyOverload_NullAssembly_ThrowsArgumentNullException()
    {
        System.Reflection.Assembly assembly = null;

        Ensure.Throws<ArgumentNullException>(() => assembly.CreateInstancesFromNamespace<ISampleService>("anything"));
    }

    [Test]
    public void CreateInstancesFromNamespace_AssemblyOverload_FindsConcreteImplementations()
    {
        var assembly = typeof(ReflectionExtensionsTests).Assembly;

        var result = assembly.CreateInstancesFromNamespace<ISampleService>(typeof(ReflectionExtensionsTests).Namespace);

        Assert.That(result.Select(r => r.GetType()), Does.Contain(typeof(SampleServiceA)));
        Assert.That(result.Select(r => r.GetType()), Does.Contain(typeof(SampleServiceB)));
    }
}
