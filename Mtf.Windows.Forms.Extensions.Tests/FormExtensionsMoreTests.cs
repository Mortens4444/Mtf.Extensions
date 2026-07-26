using System.Drawing;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class FormExtensionsMoreTests
{
    [Test]
    public void SetFormSizeAndPosition_NullForm_ThrowsArgumentNullException()
    {
        Form form = null;

        Assert.Throws<ArgumentNullException>(() => form.SetFormSizeAndPosition(new Rectangle(1, 2, 300, 200)));
    }

    [Test]
    public void SetFormSizeAndPosition_SetsLocationAndSizeFromRectangle()
    {
        using var form = new Form();

        form.SetFormSizeAndPosition(new Rectangle(10, 20, 300, 200));

        Assert.That(form.Location, Is.EqualTo(new Point(10, 20)));
        Assert.That(form.Size, Is.EqualTo(new Size(300, 200)));
    }

    [Test]
    public void IsDisposingOrDisposed_HandleCreatedHealthyForm_ReturnsFalseViaInvokePath()
    {
        using var form = new Form();
        _ = form.Handle;

        Assert.That(form.IsDisposingOrDisposed(), Is.False);
    }
}
