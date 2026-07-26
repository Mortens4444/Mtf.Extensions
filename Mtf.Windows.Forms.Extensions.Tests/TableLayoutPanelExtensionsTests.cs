using Mtf.Extensions;
using Mtf.Extensions.Interfaces;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class TableLayoutPanelExtensionsTests
{
    private sealed class GridPosition : IGridPosition
    {
        public int Column { get; init; }
        public int Row { get; init; }
        public int ColumnSpan { get; init; } = 1;
        public int RowSpan { get; init; } = 1;
    }

    [Test]
    public void SetRowsAndColumns_NullPanel_DoesNotThrow()
    {
        TableLayoutPanel panel = null;

        Ensure.DoesNotThrow(() => panel.SetRowsAndColumns(2, 3));
    }

    [Test]
    public void SetRowsAndColumns_SetsCountsAndEqualStyles()
    {
        using var panel = new TableLayoutPanel();

        panel.SetRowsAndColumns(2, 4);

        Assert.That(panel.RowCount, Is.EqualTo(2));
        Assert.That(panel.ColumnCount, Is.EqualTo(4));
        Assert.That(panel.RowStyles, Has.Count.EqualTo(2));
        Assert.That(panel.ColumnStyles, Has.Count.EqualTo(4));
        foreach (ColumnStyle style in panel.ColumnStyles)
        {
            Assert.That(style.Width, Is.EqualTo(25f).Within(0.01f));
        }
    }

    [Test]
    public void SetEqualRowsAndColumns_NullPanel_DoesNotThrow()
    {
        TableLayoutPanel panel = null;

        Ensure.DoesNotThrow(() => panel.SetEqualRowsAndColumns());
    }

    [Test]
    public void SetEqualRowsAndColumns_ZeroRowsAndColumns_DoesNotThrow()
    {
        using var panel = new TableLayoutPanel { RowCount = 0, ColumnCount = 0 };

        Ensure.DoesNotThrow(() => panel.SetEqualRowsAndColumns());
    }

    // These two calls use explicit static syntax for the same shadowing reason noted below.

    [Test]
    public void AddControl_NullPanel_DoesNotThrow()
    {
        TableLayoutPanel panel = null;
        using var control = new Control();

        Ensure.DoesNotThrow(() => TableLayoutPanelExtensions.AddControl(panel, control, new GridPosition()));
    }

    [Test]
    public void AddControl_NullGridPosition_DoesNotThrow()
    {
        using var panel = new TableLayoutPanel();
        using var control = new Control();

        Ensure.DoesNotThrow(() => TableLayoutPanelExtensions.AddControl(panel, control, null));
    }

    [Test]
    public void AddControl_PlacesControlAtSpecifiedGridPositionWithSpans()
    {
        using var panel = new TableLayoutPanel { RowCount = 3, ColumnCount = 3 };
        using var control = new Control();
        var position = new GridPosition { Column = 1, Row = 2, ColumnSpan = 2, RowSpan = 1 };

        // Called via explicit static syntax: panel.AddControl(...) here would actually bind to
        // Mtf.Windows.Forms.Extensions.ControlExtensions.AddControl(this Control, ...) instead,
        // because this test's namespace is nested under Mtf.Windows.Forms.Extensions, which puts
        // that extension method "closer" in scope than this file's `using Mtf.Extensions;`.
        TableLayoutPanelExtensions.AddControl(panel, control, position);

        Assert.That(panel.GetColumn(control), Is.EqualTo(1));
        Assert.That(panel.GetRow(control), Is.EqualTo(2));
        Assert.That(panel.GetColumnSpan(control), Is.EqualTo(2));
        Assert.That(panel.Controls, Does.Contain(control));
    }
}
