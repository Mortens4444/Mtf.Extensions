using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ListViewExtensionsTests
{
    [Test]
    public void PopulateWithLegoMindstormEV3FolderContent_UnparsableShortEntry_DoesNotAddBlankItem()
    {
        using var listView = new ListView();
        var content = new[] { "readme.txt" };

        listView.PopulateWithLegoMindstormEV3FolderContent(content, false);

        Assert.That(listView.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public void PopulateWithLegoMindstormEV3FolderContent_ValidFileEntry_AddsExactlyOneItem()
    {
        using var listView = new ListView();
        var content = new[] { "2023-01-01 1A readme.txt" };

        listView.PopulateWithLegoMindstormEV3FolderContent(content, false);

        Assert.That(listView.Items.Count, Is.EqualTo(1));
        Assert.That(listView.Items[0].IsDirectory(), Is.False);
        Assert.That(listView.Items[0].GetFileSize(), Is.EqualTo(0x1A));
    }

    [Test]
    public void PopulateWithLegoMindstormEV3FolderContent_DirectoryEntry_AddsDirectoryItem()
    {
        using var listView = new ListView();
        var content = new[] { "SubFolder/" };

        listView.PopulateWithLegoMindstormEV3FolderContent(content, false);

        Assert.That(listView.Items.Count, Is.EqualTo(1));
        Assert.That(listView.Items[0].IsDirectory(), Is.True);
    }

    [Test]
    public void HasElementWithTag_ItemWithNullTag_DoesNotThrow()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x"));

        Ensure.DoesNotThrow(() => listView.HasElementWithTag("something"));
        Assert.That(listView.HasElementWithTag("something"), Is.False);
    }

    [Test]
    public void HasElementWithTag_ItemWithMatchingTag_ReturnsTrue()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x") { Tag = "match" });

        Assert.That(listView.HasElementWithTag("match"), Is.True);
    }
}
