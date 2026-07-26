using System.Net;
using System.Net.Sockets;

namespace Mtf.Extensions.Tests;

public class SocketExtensionsTests
{
    [Test]
    public void CloseSocket_ConnectedSocket_ActuallyClosesAndDisposes()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(IPAddress.Loopback, port);
        using var server = listener.AcceptSocket();
        listener.Stop();

        Assert.That(client.Connected, Is.True);

        client.CloseSocket();

        Assert.Throws<ObjectDisposedException>(() => _ = client.Available);
    }

    [Test]
    public void CloseSocket_NullSocket_DoesNotThrow()
    {
        Socket socket = null;
        Assert.DoesNotThrow(() => socket.CloseSocket());
    }

    [Test]
    public void CloseSocket_NeverConnectedSocket_DoesNotThrowAndDisposes()
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Assert.DoesNotThrow(() => socket.CloseSocket());
        Assert.Throws<ObjectDisposedException>(() => _ = socket.Available);
    }
}
