using Mtf.Extensions.Models;
using System.Xml;

namespace Mtf.Extensions.Tests;

public class XmlDocumentExtensionsTests
{
    [Test]
    public void InitializeXmlNamespaceManager_RegistersAllNamespaces()
    {
        var xmlDocument = new XmlDocument();
        var namespaces = new List<XmlNamespace>
        {
            new("soap", "http://schemas.xmlsoap.org/soap/envelope/"),
            new("xsi", "http://www.w3.org/2001/XMLSchema-instance")
        };

        var manager = xmlDocument.InitializeXmlNamespaceManager(namespaces);

        Assert.That(manager.LookupNamespace("soap"), Is.EqualTo("http://schemas.xmlsoap.org/soap/envelope/"));
        Assert.That(manager.LookupNamespace("xsi"), Is.EqualTo("http://www.w3.org/2001/XMLSchema-instance"));
    }

    [Test]
    public void InitializeXmlNamespaceManager_EmptyList_ReturnsUsableManager()
    {
        var xmlDocument = new XmlDocument();

        var manager = xmlDocument.InitializeXmlNamespaceManager(new List<XmlNamespace>());

        Assert.That(manager, Is.Not.Null);
    }
}
