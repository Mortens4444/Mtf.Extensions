using System.Drawing;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

/// <summary>
/// Regression tests for the defects found in the code review of the "Fix bugs and add tests" batch.
/// </summary>
[TestFixture]
[Apartment(ApartmentState.STA)]
public class BugFixRegressionTests
{
    [Test]
    public void SetOsdText_DesignerAssignedSharedFont_IsNotDisposed()
    {
        using var sharedFont = new Font("Arial", 9f);
        using var form = new Form { Font = sharedFont };
        using var sibling = new Label { Font = sharedFont };
        using var target = new PictureBox { Font = sharedFont };
        form.Controls.Add(sibling);
        form.Controls.Add(target);

        target.SetOsdText("Arial", 12f, FontStyle.Bold, Color.Red, "OSD");

        // Using the shared font afterwards must still work; a disposed Font throws ArgumentException here.
        Ensure.DoesNotThrow(() => _ = sharedFont.Height);
        Ensure.DoesNotThrow(() => _ = sibling.Font.Height);
        Assert.That(target.Font.Size, Is.EqualTo(12f).Within(0.01f));
    }

    [Test]
    public void SetOsdText_PreviouslyCreatedOsdFont_IsDisposedOnReplace()
    {
        using var control = new Control();
        control.SetOsdText("Arial", 10f, FontStyle.Regular, Color.Red, "a");
        var firstOsdFont = control.Font;

        control.SetOsdText("Arial", 12f, FontStyle.Regular, Color.Red, "b");

        Ensure.Throws<ArgumentException>(() => _ = firstOsdFont.Height);
        Assert.That(control.Font.Size, Is.EqualTo(12f).Within(0.01f));
    }

    [Test]
    public void IsDisposingOrDisposed_FormOwnedByExitedThread_DoesNotThrow()
    {
        Form form = null;
        var created = new ManualResetEventSlim(false);
        var thread = new Thread(() =>
        {
            form = new Form();
            _ = form.Handle;
            created.Set();
        })
        {
            IsBackground = true
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.That(created.Wait(TimeSpan.FromSeconds(5)), Is.True);
        thread.Join();

        // The owning thread is gone, so Control.Invoke would throw InvalidAsynchronousStateException.
        bool result = default;
        Ensure.DoesNotThrow(() => result = form.IsDisposingOrDisposed());
        Assert.That(result, Is.False);
    }

    [Test]
    public void InvokeIfRequired_ControlDisposedDuringCall_DoesNotThrow()
    {
        var control = new Control();
        _ = control.Handle;

        Ensure.DoesNotThrow(() => control.InvokeIfRequired(() => control.Dispose()));
        Assert.That(control.IsDisposed, Is.True);
    }
}
