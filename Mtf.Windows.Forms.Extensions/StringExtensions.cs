using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class StringExtensions
    {
        public static void SimulateKeys(this string value)
        {
            SendKeys.Send(value);
        }
    }
}
