using Mtf.Extensions.Interfaces;
using System.Drawing;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ControlExtensionsTests
{
    private sealed class GridPosition : IGridPosition
    {
        public int Column { get; init; }
        public int Row { get; init; }
        public int ColumnSpan { get; init; } = 1;
        public int RowSpan { get; init; } = 1;
    }

    [Test]
    public void AddControl_NullContainer_ThrowsArgumentNullException()
    {
        Control container = null;
        using var control = new Control();

        Assert.Throws<ArgumentNullException>(() => container.AddControl(control, new GridPosition()));
    }

    [Test]
    public void AddControl_PlainControlContainer_AddsToControlsCollection()
    {
        using var container = new Panel();
        using var control = new Control();

        container.AddControl(control, new GridPosition());

        Assert.That(container.Controls, Does.Contain(control));
    }

    [Test]
    public void AddControl_TableLayoutPanelWithGridPosition_DelegatesToTableLayoutPanelExtensionsWithoutInfiniteRecursion()
    {
        using var panel = new TableLayoutPanel { RowCount = 3, ColumnCount = 3 };
        using var control = new Control();
        Control container = panel; // statically typed as Control, so this exercises ControlExtensions.AddControl itself
        var position = new GridPosition { Column = 1, Row = 2 };

        Assert.DoesNotThrow(() => container.AddControl(control, position));

        Assert.That(panel.GetColumn(control), Is.EqualTo(1));
        Assert.That(panel.GetRow(control), Is.EqualTo(2));
    }

    [Test]
    public void AddControl_TableLayoutPanelWithNullGridPosition_FallsBackToPlainAdd()
    {
        using var panel = new TableLayoutPanel();
        using var control = new Control();
        Control container = panel;

        container.AddControl(control, null);

        Assert.That(panel.Controls, Does.Contain(control));
    }

    [Test]
    public void SafeDispose_ControlWithoutCreatedHandle_DoesNotThrow()
    {
        var control = new Control();

        Assert.DoesNotThrow(() => control.SafeDispose());
        Assert.That(control.IsDisposed, Is.True);
    }

    [Test]
    public void SetImage_NullImageWithCloning_DoesNotThrow()
    {
        using var pictureBox = new PictureBox();
        _ = pictureBox.Handle; // force handle creation so InvokeIfRequired actually runs the action

        Assert.DoesNotThrow(() => pictureBox.SetImage(null, true));
    }

    [Test]
    public void SetImage_NullImageWithTextSetAndNoCloning_DoesNotThrow()
    {
        using var pictureBox = new PictureBox { Text = "Some text" };
        _ = pictureBox.Handle;

        Assert.DoesNotThrow(() => pictureBox.SetImage(null, false));
    }

    [Test]
    public void SetImage_PictureBoxWithClone_AssignsClonedImageAndDisposesOld()
    {
        using var pictureBox = new PictureBox();
        _ = pictureBox.Handle;
        using var firstImage = new Bitmap(2, 2);
        pictureBox.SetImage(firstImage, true);
        var firstClone = pictureBox.Image;

        using var secondImage = new Bitmap(3, 3);
        pictureBox.SetImage(secondImage, true);

        // useClone means the method never takes ownership of the caller's original image -
        // it clones it internally, so the caller's own reference must stay valid/undisposed.
        Assert.That(pictureBox.Image, Is.Not.SameAs(secondImage));
        Assert.That(pictureBox.Image.Width, Is.EqualTo(3));
        Assert.That(firstImage.Width, Is.EqualTo(2));

        // ...but the internal clone from the first call is no longer needed and gets disposed.
        Assert.Throws<ArgumentException>(() => _ = firstClone.Size);
    }

    [Test]
    public void SetImage_PictureBoxWithTextAndImage_DrawsTextWithoutThrowing()
    {
        using var pictureBox = new PictureBox { Text = "Overlay" };
        _ = pictureBox.Handle;

        Assert.DoesNotThrow(() => pictureBox.SetImage(new Bitmap(10, 10), false));
        Assert.That(pictureBox.Image, Is.Not.Null);
    }

    [Test]
    public void SetImage_PlainControlWithImage_SetsBackgroundImage()
    {
        using var control = new Control();
        _ = control.Handle;

        control.SetImage(new Bitmap(4, 4), false);

        Assert.That(control.BackgroundImage, Is.Not.Null);
    }

    [Test]
    public void SetOsdText_CalledRepeatedly_DoesNotThrowAndAppliesLatestFont()
    {
        using var control = new Control();

        Assert.DoesNotThrow(() =>
        {
            control.SetOsdText("Arial", 10f, FontStyle.Regular, Color.Red, "Hello");
            control.SetOsdText("Arial", 12f, FontStyle.Bold, Color.Blue, "World");
            control.SetOsdText("Arial", 14f, FontStyle.Italic, Color.Green, "!");
        });

        Assert.That(control.Font.Size, Is.EqualTo(14f).Within(0.01f));
        Assert.That(control.Text, Is.EqualTo("!"));
    }

    [Test]
    public void InvokeIfRequired_OnOwningThread_ExecutesSynchronously()
    {
        using var control = new Control();
        _ = control.Handle;
        var executed = false;

        control.InvokeIfRequired(() => executed = true);

        Assert.That(executed, Is.True);
    }

    [Test]
    public void InvokeIfRequired_FromBackgroundThread_BlocksUntilActionCompletes()
    {
        using var handleCreated = new ManualResetEventSlim(false);
        Control control = null;
        Form uiForm = null;

        var uiThread = new Thread(() =>
        {
            uiForm = new Form();
            control = new Control();
            uiForm.Controls.Add(control);
            _ = control.Handle;
            handleCreated.Set();
            Application.Run(uiForm);
        })
        {
            IsBackground = true
        };
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();

        try
        {
            Assert.That(handleCreated.Wait(TimeSpan.FromSeconds(5)), Is.True, "UI thread did not start in time");

            var executed = false;
            control.InvokeIfRequired(() =>
            {
                Thread.Sleep(50);
                executed = true;
            });

            // If InvokeIfRequired truly blocks (uses Invoke, not fire-and-forget InvokeAsync),
            // the flag must already be set by the time the call returns.
            Assert.That(executed, Is.True);
        }
        finally
        {
            control.Invoke((Action)(() => uiForm.Close()));
        }
    }
}
