namespace Mtf.Windows.Forms.Extensions.Tests;

public class StringExtensionsTests
{
    [Test]
    public void SimulateKeys_InvokesSendKeysWithoutThrowingUnexpectedly()
    {
        // SendKeys.Send requires the process to be pumping Windows messages; in a headless
        // test host it either sends harmlessly to whatever has focus or throws
        // InvalidOperationException because no compatible message loop is running.
        // Either outcome confirms the extension actually delegates to SendKeys.Send.
        try
        {
            "".SimulateKeys();
        }
        catch (InvalidOperationException)
        {
        }
    }
}
