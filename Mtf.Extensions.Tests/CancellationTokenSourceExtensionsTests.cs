namespace Mtf.Extensions.Tests;

public class CancellationTokenSourceExtensionsTests
{
    [Test]
    public void CancelAndDispose_CancelsTheToken()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        cts.CancelAndDispose();

        Assert.That(token.IsCancellationRequested, Is.True);
    }

    [Test]
    public void CancelAndDispose_DisposesTheSource()
    {
        var cts = new CancellationTokenSource();

        cts.CancelAndDispose();

        Assert.Throws<ObjectDisposedException>(() => _ = cts.Token);
    }

    [Test]
    public void CancelAndDispose_NullSource_DoesNotThrow()
    {
        CancellationTokenSource cts = null;
        Assert.DoesNotThrow(() => cts.CancelAndDispose());
    }
}
