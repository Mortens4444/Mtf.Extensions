using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Mtf.Extensions
{
    public static class ZipFileExtensions
    {
        public static MemoryStream GetFile(this ZipArchive zipArchive, string filename)
        {
            var zipEntry = zipArchive.GetEntry(filename);
            if (zipEntry == null)
            {
                throw new FileNotFoundException($"File '{filename}' not found in the ZIP archive.");
            }

            var contentStream = new MemoryStream();
            using (var entryStream = zipEntry.Open())
            {
                entryStream.CopyTo(contentStream);
            }

            contentStream.Seek(0, SeekOrigin.Begin);
            return contentStream;
        }

        public static XmlDocument GetXmlDocument(this ZipArchive zipArchive, string filename)
        {
            var contentXml = new XmlDocument
            {
                XmlResolver = null
            };
            var readerSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };
            using (var stream = zipArchive.GetFile(filename))
            using (var reader = XmlReader.Create(stream, readerSettings))
            {
                contentXml.Load(reader);
            }
            return contentXml;
        }
    }
}
