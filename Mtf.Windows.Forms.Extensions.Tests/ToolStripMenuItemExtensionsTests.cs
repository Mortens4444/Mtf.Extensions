using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ToolStripMenuItemExtensionsTests
{
    private enum TestMenuEnum
    {
        First,
        Second
    }

    [Test]
    public void FillWithEnum_NullMenuItem_ThrowsArgumentNullExceptionInsteadOfNullReference()
    {
        ToolStripMenuItem menuItem = null;

        Ensure.Throws<ArgumentNullException>(() => menuItem.FillWithEnum<TestMenuEnum>());
    }

    [Test]
    public void FillWithItems_NullMenuItem_ThrowsArgumentNullException()
    {
        ToolStripMenuItem menuItem = null;

        Ensure.Throws<ArgumentNullException>(() => menuItem.FillWithItems(new[] { "a", "b" }));
    }

    [Test]
    public void FillWithItems_NullItems_ThrowsArgumentNullException()
    {
        using var menuItem = new ToolStripMenuItem();

        Ensure.Throws<ArgumentNullException>(() => menuItem.FillWithItems<string>(null));
    }

    [Test]
    public void FillWithEnum_ValidMenuItem_PopulatesDropDownItems()
    {
        using var menuItem = new ToolStripMenuItem();

        menuItem.FillWithEnum<TestMenuEnum>();

        Assert.That(menuItem.DropDownItems.Count, Is.EqualTo(2));
    }
}
