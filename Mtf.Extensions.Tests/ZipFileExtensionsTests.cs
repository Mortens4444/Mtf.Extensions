using System.IO.Compression;
using System.Xml;

namespace Mtf.Extensions.Tests;

public class ZipFileExtensionsTests
{
    private static ZipArchive CreateReadableArchiveWithXml(string entryName, string xmlContent)
    {
        var ms = new MemoryStream();
        using (var writeArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            var entry = writeArchive.CreateEntry(entryName);
            using var writer = new StreamWriter(entry.Open());
            writer.Write(xmlContent);
        }

        ms.Position = 0;
        return new ZipArchive(ms, ZipArchiveMode.Read);
    }

    [Test]
    public void GetXmlDocument_ValidXml_LoadsSuccessfully()
    {
        using var archive = CreateReadableArchiveWithXml("data.xml", "<root><child>value</child></root>");

        var xmlDoc = archive.GetXmlDocument("data.xml");

        Assert.That(xmlDoc.DocumentElement?.Name, Is.EqualTo("root"));
    }

    [Test]
    public void GetXmlDocument_XmlWithDtd_ThrowsInsteadOfExpandingEntities()
    {
        const string maliciousXml = "<?xml version=\"1.0\"?><!DOCTYPE root [<!ENTITY foo \"bar\">]><root>&foo;</root>";
        using var archive = CreateReadableArchiveWithXml("data.xml", maliciousXml);

        Ensure.Throws<XmlException>(() => archive.GetXmlDocument("data.xml"));
    }
}
