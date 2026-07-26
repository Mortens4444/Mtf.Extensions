using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ListViewItemExtensionsMoreTests
{
    [Test]
    public void ConvertToString_NullItem_ReturnsEmptyString()
    {
        ListViewItem item = null;

        Assert.That(item.ConvertToString(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ConvertToString_JoinsTextAndSubItemsWithTabs()
    {
        var item = new ListViewItem("File1");
        item.SubItems.Add("100");
        item.SubItems.Add("DIR");

        Assert.That(item.ConvertToString(), Is.EqualTo("File1\t100\tDIR"));
    }

    [Test]
    public void ChangeWorkingDirectory_FileItem_ReturnsCurrentDirectoryUnchanged()
    {
        var item = new ListViewItem("readme.txt");
        item.SubItems.Add(string.Empty);
        item.SubItems.Add("100");

        Assert.That(item.ChangeWorkingDirectory("/home"), Is.EqualTo("/home"));
    }

    [Test]
    public void ChangeWorkingDirectory_ParentDirectoryEntry_NavigatesUpOneLevel()
    {
        var item = new ListViewItem(ListViewExtensions.ParentDirectory);
        item.SubItems.Add(ListViewExtensions.Directory);

        Assert.That(item.ChangeWorkingDirectory("/home/user"), Is.EqualTo("/home"));
    }

    [Test]
    public void ChangeWorkingDirectory_ParentDirectoryAtRoot_ReturnsRoot()
    {
        var item = new ListViewItem(ListViewExtensions.ParentDirectory);
        item.SubItems.Add(ListViewExtensions.Directory);

        Assert.That(item.ChangeWorkingDirectory("/home"), Is.EqualTo("/"));
    }

    [Test]
    public void ChangeWorkingDirectory_SubdirectoryEntry_AppendsToCurrentDirectory()
    {
        var item = new ListViewItem("subfolder");
        item.SubItems.Add(ListViewExtensions.Directory);

        Assert.That(item.ChangeWorkingDirectory("/home"), Is.EqualTo("/home/subfolder"));
    }

    [Test]
    public void ChangeWorkingDirectory_CurrentDirectoryAlreadyEndsWithSlash_DoesNotDoubleSlash()
    {
        var item = new ListViewItem("subfolder");
        item.SubItems.Add(ListViewExtensions.Directory);

        Assert.That(item.ChangeWorkingDirectory("/home/"), Is.EqualTo("/home/subfolder"));
    }
}
