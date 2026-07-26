using System.Drawing;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class PictureBoxExtensionsTests
{
    [Test]
    public void SetImage_SameImageReferenceAsCurrent_DoesNotDisposeIt()
    {
        using var pictureBox = new PictureBox();
        var bitmap = new Bitmap(1, 1);
        pictureBox.SetImage(bitmap);

        Ensure.DoesNotThrow(() => pictureBox.SetImage(pictureBox.Image));
        Ensure.DoesNotThrow(() => _ = pictureBox.Image.Size);
    }

    [Test]
    public void SetImage_DifferentImage_DisposesThePreviousOne()
    {
        using var pictureBox = new PictureBox();
        var firstBitmap = new Bitmap(1, 1);
        pictureBox.SetImage(firstBitmap);

        using var secondBitmap = new Bitmap(2, 2);
        pictureBox.SetImage(secondBitmap);

        Ensure.Throws<ArgumentException>(() => _ = firstBitmap.Size);
    }
}
