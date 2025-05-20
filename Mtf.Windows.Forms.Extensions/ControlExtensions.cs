using Mtf.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class ControlExtensions
    {
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
                    tableLayoutPanel.AddControl(control, gridPosition);
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
    }
}
