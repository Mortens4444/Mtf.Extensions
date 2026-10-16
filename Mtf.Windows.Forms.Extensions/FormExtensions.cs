using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class FormExtensions
    {
        public static void SetFormSizeAndPosition(this Form form, Rectangle rectangle)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            form.Location = new Point(rectangle.X, rectangle.Y);
            form.Size = new Size(rectangle.Width, rectangle.Height);
        }

        public static bool IsDisposingOrDisposed(this Form form)
        {
            // Disposing and IsDisposed are plain field reads that are safe from any thread,
            // so no Invoke is needed (and none of its thread-state exceptions can escape).
            return form == null || form.Disposing || form.IsDisposed;
        }
    }
}
