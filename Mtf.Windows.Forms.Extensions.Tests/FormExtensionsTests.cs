using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class FormExtensionsTests
{
    [Test]
    public void IsDisposingOrDisposed_HealthyFormWithoutHandle_ReturnsFalseInsteadOfTrue()
    {
        using var form = new Form();

        Assert.That(form.IsDisposingOrDisposed(), Is.False);
    }

    [Test]
    public void IsDisposingOrDisposed_DisposedForm_ReturnsTrue()
    {
        var form = new Form();
        form.Dispose();

        Assert.That(form.IsDisposingOrDisposed(), Is.True);
    }
}
