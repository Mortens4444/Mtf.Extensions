using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class PictureBoxExtensions
    {
        public static void InvokeAction(this PictureBox pictureBox, Action action)
        {
            try
            {
                if (pictureBox != null && pictureBox.IsHandleCreated)
                {
                    pictureBox.Invoke(action);
                }
            }
            catch (ObjectDisposedException) { }
            catch (ThreadAbortException) { }
            catch (Exception ex) { }
        }

        public static void ThreadSafeSetImageWithCloning(this PictureBox pictureBox, Image originalImage, object sync)
        {
            try
            {
                var image = (Image)originalImage?.Clone();
                pictureBox.ThreadSafeSetImage(image, sync);
            }
            catch { }
        }

        public static void ThreadSafeSetImage(this PictureBox pictureBox, Image image, object sync)
        {
            lock (sync)
            {
                pictureBox.SetImage(image);
            }
        }

        public static void ThreadSafeClearImage(this PictureBox pictureBox, object sync)
        {
            lock (sync)
            {
                pictureBox.SetImage(null);
            }
        }

        public static void SetImage(this PictureBox pictureBox, Image image)
        {
            try
            {
                if (pictureBox != null)
                {
                    if (!ReferenceEquals(pictureBox.Image, image))
                    {
                        pictureBox.Image?.Dispose();
                    }
                    pictureBox.Image = image;
                }
            }
            catch { }
        }

        public static void ClearImage(this PictureBox pictureBox)
        {
            pictureBox.SetImage(null);
        }

        public static void LoadImage(this PictureBox pictureBox, string path)
        {
            if (pictureBox == null)
            {
                throw new ArgumentNullException(nameof(pictureBox));
            }

            if (File.Exists(path))
            {
                try
                {
                    using (var sourceImage = Image.FromFile(path))
                    {
                        pictureBox.Image = (Bitmap)sourceImage.Clone();
                    }
                }
                catch { }
            }
        }
    }
}
