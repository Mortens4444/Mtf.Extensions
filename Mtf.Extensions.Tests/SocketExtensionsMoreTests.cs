using Mtf.Exceptions;
using System.Net;
using System.Net.Sockets;

namespace Mtf.Extensions.Tests;

public class SocketExtensionsMoreTests
{
    [Test]
    public void GetLocalIPAddresses_NullSocket_ReturnsEmpty()
    {
        Socket socket = null;

        var result = socket.GetLocalIPAddresses(_ => new[] { "should-not-be-called" });

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetLocalIPAddresses_UnboundSocket_ReturnsEmpty()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        var result = socket.GetLocalIPAddresses(_ => new[] { "should-not-be-called" });

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetLocalIPAddresses_BoundToSpecificAddress_ReturnsThatAddress()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));

        var result = socket.GetLocalIPAddresses(_ => new[] { "should-not-be-called" }).ToList();

        Assert.That(result, Is.EqualTo(new[] { "127.0.0.1" }));
    }

    [Test]
    public void GetLocalIPAddresses_BoundToAnyAddress_DelegatesToCallback()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 0));

        var result = socket.GetLocalIPAddresses(_ => new[] { "from-callback" }).ToList();

        Assert.That(result, Is.EqualTo(new[] { "from-callback" }));
    }

    [Test]
    public void GetLocalIPAddressesInfo_NullSocket_ReturnsNull()
    {
        Socket socket = null;

        var result = socket.GetLocalIPAddressesInfo(_ => new[] { "x" });

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetLocalIPAddressesInfo_BoundToSpecificAddress_ReturnsEndpointText()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));

        var result = socket.GetLocalIPAddressesInfo(_ => new[] { "x" });

        Assert.That(result, Does.StartWith("127.0.0.1:"));
    }

    [Test]
    public void IsSocketConnected_NullSocket_ReturnsFalse()
    {
        Socket socket = null;

        Assert.That(socket.IsSocketConnected(), Is.False);
    }

    [Test]
    public void IsSocketConnected_UnconnectedSocket_ReturnsFalse()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Assert.That(socket.IsSocketConnected(), Is.False);
    }

    [Test]
    public void IsSocketConnected_ConnectedSocket_ReturnsTrue()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(IPAddress.Loopback, port);
        using var server = listener.AcceptSocket();
        listener.Stop();

        Assert.That(client.IsSocketConnected(), Is.True);
    }

    [Test]
    public void Connect_NullSocket_ThrowsArgumentNullException()
    {
        Socket socket = null;

        Assert.Throws<ArgumentNullException>(() => socket.Connect("127.0.0.1", 12345, 1000, _ => Enumerable.Empty<string>()));
    }

    [Test]
    public void Connect_AlreadyConnectedSocket_ReturnsWithoutReconnecting()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(IPAddress.Loopback, port);
        using var server = listener.AcceptSocket();
        listener.Stop();

        Assert.DoesNotThrow(() => client.Connect("127.0.0.1", (ushort)port, 1000, _ => Enumerable.Empty<string>()));
    }

    [Test]
    public void Connect_SuccessfulConnection_DoesNotThrow()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Assert.DoesNotThrow(() => client.Connect("127.0.0.1", (ushort)port, 2000, _ => Enumerable.Empty<string>()));
        Assert.That(client.Connected, Is.True);

        listener.Stop();
    }

    [Test]
    public void Connect_NothingListening_ThrowsConnectionException()
    {
        // Grab a free port and immediately release it so nothing is listening there.
        var probe = new TcpListener(IPAddress.Loopback, 0);
        probe.Start();
        var freePort = ((IPEndPoint)probe.LocalEndpoint).Port;
        probe.Stop();

        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Assert.That(
            () => client.Connect("127.0.0.1", (ushort)freePort, 2000, _ => Enumerable.Empty<string>()),
            Throws.TypeOf<ConnectionFailedException>().Or.TypeOf<ConnectionTimedOutException>());
    }
}
