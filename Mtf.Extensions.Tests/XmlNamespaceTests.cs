using Mtf.Extensions.Models;

namespace Mtf.Extensions.Tests;

public class XmlNamespaceTests
{
    [Test]
    public void Constructor_SetsPrefixAndUri()
    {
        var ns = new XmlNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");

        Assert.That(ns.Prefix, Is.EqualTo("soap"));
        Assert.That(ns.Uri, Is.EqualTo("http://schemas.xmlsoap.org/soap/envelope/"));
    }

    [Test]
    public void Properties_AreSettable()
    {
        var ns = new XmlNamespace("a", "b")
        {
            Prefix = "c",
            Uri = "d"
        };

        Assert.That(ns.Prefix, Is.EqualTo("c"));
        Assert.That(ns.Uri, Is.EqualTo("d"));
    }
}
