namespace Mtf.Extensions.Tests;

public class DictionaryExtensionTests
{
    [Test]
    public void AddNotEmpty_NullDictionary_ThrowsArgumentNullException()
    {
        Dictionary<string, IEnumerable<int>> dictionary = null;

        Assert.Throws<ArgumentNullException>(() => dictionary.AddNotEmpty("key", new[] { 1 }));
    }

    [Test]
    public void AddNotEmpty_NullElements_DoesNotAddKey()
    {
        var dictionary = new Dictionary<string, IEnumerable<int>>();

        dictionary.AddNotEmpty("key", null);

        Assert.That(dictionary.ContainsKey("key"), Is.False);
    }

    [Test]
    public void AddNotEmpty_EmptyElements_DoesNotAddKey()
    {
        var dictionary = new Dictionary<string, IEnumerable<int>>();

        dictionary.AddNotEmpty("key", Array.Empty<int>());

        Assert.That(dictionary.ContainsKey("key"), Is.False);
    }

    [Test]
    public void AddNotEmpty_NonEmptyElements_AddsKey()
    {
        var dictionary = new Dictionary<string, IEnumerable<int>>();

        dictionary.AddNotEmpty("key", new[] { 1, 2, 3 });

        Assert.That(dictionary.ContainsKey("key"), Is.True);
        Assert.That(dictionary["key"], Is.EquivalentTo(new[] { 1, 2, 3 }));
    }
}
