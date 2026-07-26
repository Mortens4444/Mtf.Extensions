using Mtf.Extensions.Interfaces;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ListViewExtensionsMoreTests
{
    private sealed class GuidOwner : IHaveGuid
    {
        public string Guid { get; init; }
    }

    private sealed class IdOwner : IHaveId<long>
    {
        public long Id { get; init; }
    }

    [Test]
    public void HasElementWithGuid_MatchingTag_ReturnsTrue()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x") { Tag = new GuidOwner { Guid = "abc-123" } });

        Assert.That(listView.HasElementWithGuid("abc-123"), Is.True);
    }

    [Test]
    public void HasElementWithGuid_NoMatch_ReturnsFalse()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x") { Tag = new GuidOwner { Guid = "abc-123" } });

        Assert.That(listView.HasElementWithGuid("different"), Is.False);
    }

    [Test]
    public void HasElementWithGuid_TagIsNotIHaveGuid_ReturnsFalse()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x") { Tag = "plain string" });

        Assert.That(listView.HasElementWithGuid("abc-123"), Is.False);
    }

    [Test]
    public void HasElementWithId_MatchingTag_ReturnsTrue()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x") { Tag = new IdOwner { Id = 42 } });

        Assert.That(listView.HasElementWithId(42), Is.True);
    }

    [Test]
    public void HasElementWithId_NoMatch_ReturnsFalse()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("x") { Tag = new IdOwner { Id = 42 } });

        Assert.That(listView.HasElementWithId(99), Is.False);
    }

    [Test]
    public void SelectAll_ItemCollectionOverload_SelectsEveryItem()
    {
        using var listView = new ListView { MultiSelect = true };
        _ = listView.Handle;
        listView.Items.Add(new ListViewItem("a"));
        listView.Items.Add(new ListViewItem("b"));

        listView.Items.SelectAll();

        Assert.That(listView.Items[0].Selected, Is.True);
        Assert.That(listView.Items[1].Selected, Is.True);
    }

    [Test]
    public void SelectAll_GroupOverload_SelectsEveryItemInGroup()
    {
        using var listView = new ListView { MultiSelect = true };
        _ = listView.Handle;
        var group = new ListViewGroup("Group1");
        listView.Groups.Add(group);
        var item1 = new ListViewItem("a") { Group = group };
        var item2 = new ListViewItem("b") { Group = group };
        listView.Items.Add(item1);
        listView.Items.Add(item2);

        group.SelectAll();

        Assert.That(item1.Selected, Is.True);
        Assert.That(item2.Selected, Is.True);
    }

    [Test]
    public void AddItems_ClearsExistingAndConvertsEachItem()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("old"));

        listView.AddItems(new[] { "a", "b", "c" }, text => new ListViewItem(text));

        Assert.That(listView.Items.Count, Is.EqualTo(3));
        Assert.That(listView.Items[0].Text, Is.EqualTo("a"));
    }

    [Test]
    public void AddItems_ClearItemsFalse_AppendsToExisting()
    {
        using var listView = new ListView();
        listView.Items.Add(new ListViewItem("old"));

        listView.AddItems(new[] { "a" }, text => new ListViewItem(text), clearItems: false);

        Assert.That(listView.Items.Count, Is.EqualTo(2));
    }
}
