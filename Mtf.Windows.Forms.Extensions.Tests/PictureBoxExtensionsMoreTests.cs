using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class PictureBoxExtensionsMoreTests
{
    [Test]
    public void InvokeAction_NoHandleCreated_DoesNotThrow()
    {
        using var pictureBox = new PictureBox();
        var executed = false;

        Ensure.DoesNotThrow(() => pictureBox.InvokeAction(() => executed = true));
        Assert.That(executed, Is.False);
    }

    [Test]
    public void InvokeAction_HandleCreated_ExecutesAction()
    {
        using var pictureBox = new PictureBox();
        _ = pictureBox.Handle;
        var executed = false;

        pictureBox.InvokeAction(() => executed = true);

        Assert.That(executed, Is.True);
    }

    [Test]
    public void InvokeAction_NullPictureBox_DoesNotThrow()
    {
        PictureBox pictureBox = null;

        Ensure.DoesNotThrow(() => pictureBox.InvokeAction(() => { }));
    }

    [Test]
    public void ThreadSafeSetImageWithCloning_ClonesAndAssignsImage()
    {
        using var pictureBox = new PictureBox();
        using var original = new Bitmap(2, 2);
        var sync = new object();

        pictureBox.ThreadSafeSetImageWithCloning(original, sync);

        Assert.That(pictureBox.Image, Is.Not.Null);
        Assert.That(pictureBox.Image, Is.Not.SameAs(original));
    }

    [Test]
    public void ThreadSafeSetImageWithCloning_NullImage_DoesNotThrow()
    {
        using var pictureBox = new PictureBox();
        var sync = new object();

        Ensure.DoesNotThrow(() => pictureBox.ThreadSafeSetImageWithCloning(null, sync));
    }

    [Test]
    public void ThreadSafeSetImage_AssignsImageUnderLock()
    {
        using var pictureBox = new PictureBox();
        using var image = new Bitmap(2, 2);
        var sync = new object();

        pictureBox.ThreadSafeSetImage(image, sync);

        Assert.That(pictureBox.Image, Is.SameAs(image));
    }

    [Test]
    public void ThreadSafeClearImage_RemovesImage()
    {
        using var pictureBox = new PictureBox();
        pictureBox.SetImage(new Bitmap(2, 2));
        var sync = new object();

        pictureBox.ThreadSafeClearImage(sync);

        Assert.That(pictureBox.Image, Is.Null);
    }

    [Test]
    public void ClearImage_RemovesCurrentImage()
    {
        using var pictureBox = new PictureBox();
        pictureBox.SetImage(new Bitmap(2, 2));

        pictureBox.ClearImage();

        Assert.That(pictureBox.Image, Is.Null);
    }

    [Test]
    public void LoadImage_NullPictureBox_ThrowsArgumentNullException()
    {
        PictureBox pictureBox = null;

        Ensure.Throws<ArgumentNullException>(() => pictureBox.LoadImage("anything.png"));
    }

    [Test]
    public void LoadImage_NonExistentFile_DoesNothing()
    {
        using var pictureBox = new PictureBox();

        Ensure.DoesNotThrow(() => pictureBox.LoadImage(@"C:\no\such\file.png"));
        Assert.That(pictureBox.Image, Is.Null);
    }

    [Test]
    public void LoadImage_ExistingFile_LoadsClonedImage()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");
        using (var bitmap = new Bitmap(3, 3))
        {
            bitmap.Save(tempFile, ImageFormat.Png);
        }

        try
        {
            using var pictureBox = new PictureBox();

            pictureBox.LoadImage(tempFile);

            Assert.That(pictureBox.Image, Is.Not.Null);
            Assert.That(pictureBox.Image.Width, Is.EqualTo(3));
        }
        finally
        {
            // Image.FromFile can keep the file's native handle alive past Dispose() (a
            // long-standing, timing-dependent GDI+ quirk); this cleanup is incidental to what
            // the test verifies, so don't fail the test over a stray temp file.
            try
            {
                File.Delete(tempFile);
            }
            catch (IOException)
            {
            }
        }
    }
}
