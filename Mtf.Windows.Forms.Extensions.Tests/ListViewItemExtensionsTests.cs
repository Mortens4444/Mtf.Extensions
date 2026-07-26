using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ListViewItemExtensionsTests
{
    [Test]
    public void ToArrayList_NullItem_ReturnsEmptyCollectionInsteadOfThrowing()
    {
        ListViewItem item = null;

        var result = item.ToArrayList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ToStringInPreferredFormat_NullItem_ReturnsEmptyStringInsteadOfThrowing()
    {
        ListViewItem item = null;

        Assert.That(item.ToStringInPreferredFormat(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void IsDirectory_ItemWithoutEnoughSubItems_ReturnsFalseInsteadOfThrowing()
    {
        var item = new ListViewItem("foo");

        Assert.That(item.IsDirectory(), Is.False);
    }

    [Test]
    public void GetFileSize_ItemWithoutEnoughSubItems_ReturnsZeroInsteadOfThrowing()
    {
        var item = new ListViewItem("foo");

        Assert.That(item.GetFileSize(), Is.EqualTo(0));
    }

    [Test]
    public void IsDirectory_DirectoryItem_ReturnsTrue()
    {
        var item = new ListViewItem("foo");
        item.SubItems.Add(ListViewExtensions.Directory);

        Assert.That(item.IsDirectory(), Is.True);
    }

    [Test]
    public void GetFileSize_ItemWithSizeSubItem_ReturnsSize()
    {
        var item = new ListViewItem("foo");
        item.SubItems.Add(string.Empty);
        item.SubItems.Add("1234");

        Assert.That(item.GetFileSize(), Is.EqualTo(1234));
    }
}
