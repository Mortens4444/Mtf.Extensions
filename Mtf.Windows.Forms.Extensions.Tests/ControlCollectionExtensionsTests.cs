using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ControlCollectionExtensionsTests
{
    [Test]
    public void Where_NullPredicate_ReturnsEmptySequenceInsteadOfThrowing()
    {
        using var parent = new Control();

        var result = parent.Controls.Where(null).ToList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Where_NullControlsCollection_ReturnsEmptySequenceInsteadOfThrowing()
    {
        Control.ControlCollection nullControls = null;

        var result = nullControls.Where(c => true).ToList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Where_ValidPredicate_FiltersCorrectly()
    {
        using var parent = new Control();
        using var child1 = new Control { Name = "A" };
        using var child2 = new Control { Name = "B" };
        parent.Controls.Add(child1);
        parent.Controls.Add(child2);

        var result = parent.Controls.Where(c => c.Name == "A").ToList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("A"));
    }

    [Test]
    public void AnyWithPredicate_NullControlsOrPredicate_ReturnsFalse()
    {
        using var parent = new Control();

        Assert.That(parent.Controls.Any(null), Is.False);

        Control.ControlCollection nullControls = null;
        Assert.That(nullControls.Any(c => true), Is.False);
    }

    [Test]
    public void AnyWithPredicate_MatchingControlExists_ReturnsTrue()
    {
        using var parent = new Control();
        using var child = new Control { Name = "A" };
        parent.Controls.Add(child);

        Assert.That(parent.Controls.Any(c => c.Name == "A"), Is.True);
        Assert.That(parent.Controls.Any(c => c.Name == "Z"), Is.False);
    }

    [Test]
    public void AnyNoPredicate_EmptyCollection_ReturnsFalse()
    {
        using var parent = new Control();

        Assert.That(parent.Controls.Any(), Is.False);
    }

    [Test]
    public void AnyNoPredicate_NonEmptyCollection_ReturnsTrue()
    {
        using var parent = new Control();
        using var child = new Control();
        parent.Controls.Add(child);

        Assert.That(parent.Controls.Any(), Is.True);
    }

    [Test]
    public void AnyNoPredicate_NullCollection_ReturnsFalse()
    {
        Control.ControlCollection nullControls = null;

        Assert.That(nullControls.Any(), Is.False);
    }
}
