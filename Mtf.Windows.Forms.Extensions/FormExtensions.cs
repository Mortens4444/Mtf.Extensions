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
    }
}
