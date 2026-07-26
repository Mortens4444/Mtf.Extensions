using Mtf.Exceptions;

namespace Mtf.Extensions.Tests;

public class ConnectionExceptionsTests
{
    [Test]
    public void ConnectionFailedException_MessageContainsServerAddressPortAndLocalEndPoint()
    {
        var ex = new ConnectionFailedException("10.0.0.1", 8080, "192.168.0.1:12345");

        Assert.That(ex.Message, Does.Contain("10.0.0.1"));
        Assert.That(ex.Message, Does.Contain("8080"));
        Assert.That(ex.Message, Does.Contain("192.168.0.1:12345"));
    }

    [Test]
    public void ConnectionTimedOutException_MessageContainsServerAddressPortAndLocalEndPoint()
    {
        var ex = new ConnectionTimedOutException("10.0.0.1", 8080, "192.168.0.1:12345");

        Assert.That(ex.Message, Does.Contain("10.0.0.1"));
        Assert.That(ex.Message, Does.Contain("8080"));
        Assert.That(ex.Message, Does.Contain("192.168.0.1:12345"));
    }
}
