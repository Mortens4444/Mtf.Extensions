using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ControlExtensionsMoreTests
{
    [Test]
    public void ExecuteThreadSafely_NullControl_ThrowsArgumentNullException()
    {
        Control control = null;

        Ensure.Throws<ArgumentNullException>(() => control.ExecuteThreadSafely(() => { }));
    }

    [Test]
    public void ExecuteThreadSafely_OnOwningThread_ExecutesSynchronously()
    {
        using var control = new Control();
        _ = control.Handle;
        var executed = false;

        control.ExecuteThreadSafely(() => executed = true);

        Assert.That(executed, Is.True);
    }

    [Test]
    public void ExecuteThreadSafelyGeneric_NullControl_ThrowsArgumentNullException()
    {
        Control control = null;

        Ensure.Throws<ArgumentNullException>(() => control.ExecuteThreadSafely(() => 42));
    }

    [Test]
    public void ExecuteThreadSafelyGeneric_ReturnsFunctionResult()
    {
        using var control = new Control();
        _ = control.Handle;

        var result = control.ExecuteThreadSafely(() => 42);

        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public void ExecuteThreadSafelyGeneric_DisposedControlSameThread_RunsFuncDirectly()
    {
        // Control.InvokeRequired returns false for a disposed control when called from the
        // same thread that owns it (no marshaling needed), so it takes the direct-call branch
        // and never reaches the ObjectDisposedException fallback - that only triggers via an
        // actual cross-thread Invoke on a disposed control.
        var control = new Control();
        _ = control.Handle;
        control.Dispose();

        int result = 0;
        Ensure.DoesNotThrow(() => result = control.ExecuteThreadSafely(() => 42, -1));
        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public async Task InvokeAsync_NullControl_ReturnsCanceledTask()
    {
        Control control = null;

        var task = control.InvokeAsync(() => { });

        Assert.That(task.IsCanceled, Is.True);
        try
        {
            await task;
            Assert.Fail("Expected the task to be canceled.");
        }
        catch (TaskCanceledException)
        {
        }
    }

    [Test]
    public async Task InvokeAsync_ValidControl_ExecutesActionAndCompletes()
    {
        // BeginInvoke only ever runs its callback once the owning thread pumps Windows
        // messages, so this needs a real UI thread running Application.Run - calling it from
        // the same thread that created the handle (with no message loop) would hang forever.
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
            await control.InvokeAsync(() => executed = true);

            Assert.That(executed, Is.True);
        }
        finally
        {
            control.Invoke((Action)(() => uiForm.Close()));
        }
    }

    [Test]
    public void InvokeAsync_DisposedControl_ReturnsCanceledTask()
    {
        var control = new Control();
        _ = control.Handle;
        control.Dispose();

        var task = control.InvokeAsync(() => { });

        Assert.That(task.IsCanceled, Is.True);
    }
}
