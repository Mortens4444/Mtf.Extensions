using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ToolStripMenuItemExtensionsMoreTests
{
    private enum TestMenuEnum
    {
        First,
        Second
    }

    [Test]
    public void FillWithEnum_ClickingItem_InvokesCallbackWithItemAndValue()
    {
        using var menuItem = new ToolStripMenuItem();
        ToolStripMenuItem clickedItem = null;
        TestMenuEnum clickedValue = default;

        menuItem.FillWithEnum<TestMenuEnum>((item, value) =>
        {
            clickedItem = item;
            clickedValue = value;
        });

        var secondItem = (ToolStripMenuItem)menuItem.DropDownItems[1];
        secondItem.PerformClick();

        Assert.That(clickedItem, Is.SameAs(secondItem));
        Assert.That(clickedValue, Is.EqualTo(TestMenuEnum.Second));
    }

    [Test]
    public void FillWithEnum_TagsEachItemWithItsEnumValue()
    {
        using var menuItem = new ToolStripMenuItem();

        menuItem.FillWithEnum<TestMenuEnum>();

        Assert.That(((ToolStripMenuItem)menuItem.DropDownItems[0]).Tag, Is.EqualTo(TestMenuEnum.First));
        Assert.That(((ToolStripMenuItem)menuItem.DropDownItems[1]).Tag, Is.EqualTo(TestMenuEnum.Second));
    }

    [Test]
    public void FillWithItems_ClickingItem_InvokesCallbackWithItemAndValue()
    {
        using var menuItem = new ToolStripMenuItem();
        string clickedValue = null;

        menuItem.FillWithItems(new[] { "alpha", "beta" }, (item, value) => clickedValue = value);

        ((ToolStripMenuItem)menuItem.DropDownItems[0]).PerformClick();

        Assert.That(clickedValue, Is.EqualTo("alpha"));
    }

    [Test]
    public void FillWithItems_PopulatesDropDownItemsWithGivenTexts()
    {
        using var menuItem = new ToolStripMenuItem();

        menuItem.FillWithItems(new[] { "alpha", "beta" });

        Assert.That(menuItem.DropDownItems.Count, Is.EqualTo(2));
        Assert.That(menuItem.DropDownItems[0].Text, Is.EqualTo("alpha"));
        Assert.That(menuItem.DropDownItems[1].Text, Is.EqualTo("beta"));
    }

    [Test]
    public void FillWithEnum_ClearsPreviouslyPopulatedItems()
    {
        using var menuItem = new ToolStripMenuItem();
        menuItem.DropDownItems.Add("stale item");

        menuItem.FillWithEnum<TestMenuEnum>();

        Assert.That(menuItem.DropDownItems.Count, Is.EqualTo(2));
    }
}
