using Mtf.Extensions;
using Mtf.Extensions.Interfaces;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class ControlExtensions
    {
        private static readonly ConditionalWeakTable<Control, Font> osdFonts = new ConditionalWeakTable<Control, Font>();

        public static void AddControl(this Control container, Control control, IGridPosition gridPosition)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (container is TableLayoutPanel tableLayoutPanel)
            {
                if (gridPosition != null)
                {
                    TableLayoutPanelExtensions.AddControl(tableLayoutPanel, control, gridPosition);
                }
                else
                {
                    container.Controls.Add(control);
                }
            }
            else
            {
                container.Controls.Add(control);
            }
        }

        public static void ExecuteThreadSafely(this Control control, Action action)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            try
            {
                if (!control.InvokeRequired)
                {
                    action();
                }
                else
                {
                    control.Invoke(action);
                }
            }
            catch (ObjectDisposedException) { }
        }

        public static T ExecuteThreadSafely<T>(this Control control, Func<T> func, T fallback = default)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            try
            {
                return !control.InvokeRequired ? func() : (T)control.Invoke(func);
            }
            catch (ObjectDisposedException)
            {
                return fallback;
            }
        }

        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control == null || control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            try
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(action);
                }
                else
                {
                    action();
                }
            }
            catch (ObjectDisposedException)
            {
                // The control was disposed between the check above and the invoke.
            }
            catch (InvalidOperationException) when (control.IsDisposed || !control.IsHandleCreated)
            {
                // The handle was destroyed while the invoke was pending.
            }
        }

        public static void SafeDispose(this Control control)
        {
            if (control != null && !control.IsDisposed)
            {
                control.Parent?.Controls.Remove(control);
                if (control.IsHandleCreated)
                {
                    control.BeginInvoke((Action)(() =>
                    {
                        control.Dispose();
                    }));
                }
                else
                {
                    control.Dispose();
                }
            }
        }

        public static Task InvokeAsync(this Control control, Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            if (control == null || control.IsDisposed)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            control.BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    if (!control.IsDisposed)
                    {
                        action();
                        tcs.SetResult(null);
                    }
                    else
                    {
                        tcs.SetCanceled();
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }));
            return tcs.Task;
        }

        public static void SetOsdText(this Control control, string familyName, float emSize, FontStyle fontStyle, Color foreColor, string text)
        {
            if (control == null)
            {
                return;
            }

            var newFont = new Font(familyName, emSize, fontStyle);
            control.Font = newFont;

            // Only dispose fonts this method created earlier for the same control. Any other font
            // (designer-assigned, ambient, parent) may be shared, so disposing it is never safe here.
            if (osdFonts.TryGetValue(control, out var previousOsdFont))
            {
                osdFonts.Remove(control);
                previousOsdFont.Dispose();
            }
            osdFonts.Add(control, newFont);

            control.ForeColor = foreColor;
            control.Text = text;
        }

        public static void SetImage(this Control control, Image image, bool useClone)
        {
            control.InvokeIfRequired(() =>
            {
                InternalSetImage(control, image, useClone);
            });
        }

        private static void InternalSetImage(Control control, Image image, bool useClone)
        {
            Image oldImage;
            var newImage = image != null && useClone ? (Image)image.Clone() : image;
            if (control is PictureBox pictureBox)
            {
                oldImage = pictureBox.Image;
                pictureBox.Image = newImage;
                SetTextOnImage(control, pictureBox.Image);
            }
            else
            {
                oldImage = control.BackgroundImage;
                control.BackgroundImage = newImage;
                SetTextOnImage(control, control.BackgroundImage);
            }
            control.Invalidate();
            control.Update();
            if (oldImage != null && !ReferenceEquals(oldImage, image))
            {
                oldImage.Dispose();
            }
        }

        private static void SetTextOnImage(Control control, Image image)
        {
            SetTextOnImage(control, image, Color.DarkGray, 2);
        }

        private static void SetTextOnImage(Control control, Image image, Color shadowColor, int shadowOffset)
        {
            if (image == null || String.IsNullOrEmpty(control.Text))
            {
                return;
            }

            using (var g = Graphics.FromImage(image))
            {
                var textLocation = new PointF(10, 10);
                using (var shadowBrush = new SolidBrush(shadowColor))
                {
                    var shadowLocation = new PointF(textLocation.X + shadowOffset, textLocation.Y + shadowOffset);
                    g.DrawString(control.Text, control.Font, shadowBrush, shadowLocation);
                }

                using (var brush = new SolidBrush(control.ForeColor))
                {
                    g.DrawString(control.Text, control.Font, brush, textLocation);
                }
                control.Refresh();
            }
        }
    }
}
