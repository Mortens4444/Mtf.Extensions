using Mtf.Extensions.Exceptions;
using System.Net;
using System.Net.Sockets;

namespace Mtf.Extensions.Tests;

public class EndPointExtensionsTests
{
    private sealed class TextEndPoint : EndPoint
    {
        private readonly string text;

        public TextEndPoint(string text) => this.text = text;

        public override string ToString() => text;
    }

    [Test]
    public void GetPort_NoPortInEndPointText_ThrowsLocalizedExceptionInsteadOfIndexOutOfRange()
    {
        var endpoint = new TextEndPoint("justanaddress");

        Ensure.Throws<LocalizedException>(() => endpoint.GetPort());
    }

    [Test]
    public void GetIpAddressAndPort_NoPortInEndPointText_ThrowsLocalizedException()
    {
        var endpoint = new TextEndPoint("justanaddress");

        Ensure.Throws<LocalizedException>(() => endpoint.GetIpAddressAndPort());
    }

    [Test]
    public void GetPort_ValidEndPoint_ReturnsPort()
    {
        var endpoint = new IPEndPoint(IPAddress.Loopback, 8080);

        Assert.That(endpoint.GetPort(), Is.EqualTo((ushort)8080));
    }

    [Test]
    public void GetIpAddressAndPort_ValidEndPoint_ParsesBoth()
    {
        var endpoint = new IPEndPoint(IPAddress.Loopback, 8080);

        var result = endpoint.GetIpAddressAndPort();

        Assert.That(result.Item2, Is.EqualTo((ushort)8080));
    }
}
