using Mtf.Extensions;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ComboBoxExtensionsTests
{
    [Test]
    public void SelectFirst_WithItems_SelectsIndexZero()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.AddRange(new object[] { "a", "b", "c" });

        comboBox.SelectFirst();

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(0));
    }

    [Test]
    public void SelectFirst_NoItems_DoesNothing()
    {
        using var comboBox = new ComboBox();

        comboBox.SelectFirst();

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(-1));
    }

    [Test]
    public void SelectFirstOrSetDisabled_NoItems_DisablesControl()
    {
        using var comboBox = new ComboBox();

        comboBox.SelectFirstOrSetDisabled();

        Assert.That(comboBox.Enabled, Is.False);
    }

    [Test]
    public void SelectFirstOrSetDisabled_HasItems_SelectsFirstAndStaysEnabled()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.Add("a");

        comboBox.SelectFirstOrSetDisabled();

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(0));
        Assert.That(comboBox.Enabled, Is.True);
    }

    [Test]
    public void AddItems_ClearsExistingAndAddsNewOnes()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.Add("old");

        comboBox.AddItems(new object[] { "a", "b" });

        Assert.That(comboBox.Items.Count, Is.EqualTo(2));
        Assert.That(comboBox.Items[0], Is.EqualTo("a"));
    }

    [Test]
    public void AddItemsAndSelectFirst_SelectsFirstAfterAdding()
    {
        using var comboBox = new ComboBox();

        comboBox.AddItemsAndSelectFirst(new object[] { "x", "y" });

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(0));
    }

    [Test]
    public void SafeSelect_IndexWithinRange_SelectsIt()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.AddRange(new object[] { "a", "b", "c" });

        comboBox.SafeSelect(2);

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(2));
    }

    [Test]
    public void SafeSelect_IndexOutOfRange_DoesNotThrowOrChangeSelection()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.Add("a");

        Ensure.DoesNotThrow(() => comboBox.SafeSelect(5));
        Assert.That(comboBox.SelectedIndex, Is.EqualTo(-1));
    }

    // ComboBox.DataSource binding only actually populates Items once the control has a
    // BindingContext, which it normally inherits from a parent Form. A standalone control has
    // none, so these assign one directly (the standard way to unit-test data binding in isolation).

    [Test]
    public void FillAndSelect_GenericList_SetsDataSourceAndSelectedIndex()
    {
        using var comboBox = new ComboBox { BindingContext = new BindingContext() };
        var list = new List<string> { "a", "b", "c" };

        comboBox.FillAndSelect(list, 1);

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(1));
    }

    [Test]
    public void FillAndSelectFirst_GenericList_SelectsIndexZero()
    {
        using var comboBox = new ComboBox { BindingContext = new BindingContext() };
        var list = new List<string> { "a", "b" };

        comboBox.FillAndSelectFirst(list);

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(0));
    }

    [Test]
    public void FillAndSelectLast_GenericList_SelectsLastIndex()
    {
        using var comboBox = new ComboBox { BindingContext = new BindingContext() };
        var list = new List<string> { "a", "b", "c" };

        comboBox.FillAndSelectLast(list);

        Assert.That(comboBox.SelectedIndex, Is.EqualTo(2));
    }

    [Test]
    public void FillWithTypesInNamespace_PopulatesWithInstancesOfMatchingTypes()
    {
        using var comboBox = new ComboBox { BindingContext = new BindingContext() };

        comboBox.FillWithTypesInNamespace(typeof(FillWithTypesInNamespaceSample.SampleItem).Assembly, typeof(FillWithTypesInNamespaceSample.SampleItem).Namespace);

        Assert.That(comboBox.Items.Count, Is.GreaterThan(0));
    }

    [Test]
    public void IndexOf_NullComboBox_ThrowsArgumentNullException()
    {
        ComboBox comboBox = null;

        Ensure.Throws<ArgumentNullException>(() => comboBox.IndexOf("x"));
    }

    [Test]
    public void IndexOf_ReturnsPositionOfItem()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.AddRange(new object[] { "a", "b", "c" });

        Assert.That(comboBox.IndexOf("b"), Is.EqualTo(1));
    }

    [Test]
    public void GetSelectedItemThreadSafe_WithHandleAndSelection_ReturnsSelectedItem()
    {
        using var comboBox = new ComboBox();
        comboBox.Items.AddRange(new object[] { "a", "b" });
        comboBox.SelectedIndex = 1;
        _ = comboBox.Handle;

        var result = comboBox.GetSelectedItemThreadSafe();

        Assert.That(result, Is.EqualTo("b"));
    }

    [Test]
    public void GetSelectedItemThreadSafe_NoHandleCreated_ReturnsNullInsteadOfThrowing()
    {
        var comboBox = new ComboBox();

        Ensure.DoesNotThrow(() =>
        {
            var result = comboBox.GetSelectedItemThreadSafe();
            Assert.That(result, Is.Null);
        });

        comboBox.Dispose();
    }
}
