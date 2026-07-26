using System.Collections.ObjectModel;

namespace Mtf.Extensions.Tests;

public class ObservableCollectionExtensionsTests
{
    [Test]
    public void AddRange_NullCollection_ThrowsArgumentNullException()
    {
        ObservableCollection<int> collection = null;

        Ensure.Throws<ArgumentNullException>(() => collection.AddRange(new[] { 1, 2 }));
    }

    [Test]
    public void AddRange_NullItems_DoesNothing()
    {
        var collection = new ObservableCollection<int> { 1 };

        collection.AddRange(null);

        Assert.That(collection, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddRange_ValidItems_AddsThemAllInOrder()
    {
        var collection = new ObservableCollection<int> { 1 };

        collection.AddRange(new[] { 2, 3, 4 });

        Assert.That(collection, Is.EqualTo(new[] { 1, 2, 3, 4 }));
    }
}
