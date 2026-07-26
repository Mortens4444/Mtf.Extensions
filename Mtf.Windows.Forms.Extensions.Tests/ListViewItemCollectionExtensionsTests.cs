using System.Windows.Forms;
using System.Xml.Linq;

namespace Mtf.Windows.Forms.Extensions.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class ListViewItemCollectionExtensionsTests
{
    [Test]
    public void ExportContent_NullCollection_DoesNotThrow()
    {
        ListView.ListViewItemCollection collection = null;

        Assert.DoesNotThrow(() => collection.ExportContent(Path.GetTempFileName()));
    }

    [Test]
    public void ExportContent_WritesItemsAndSubItemsAsXml()
    {
        using var listView = new ListView();
        var item = new ListViewItem("File1");
        item.SubItems.Add("100");
        listView.Items.Add(item);
        listView.Items.Add(new ListViewItem("File2"));

        var tempFile = Path.GetTempFileName();
        try
        {
            listView.Items.ExportContent(tempFile);

            var document = XDocument.Load(tempFile);
            var items = document.Root.Elements("Item").ToList();

            Assert.That(items, Has.Count.EqualTo(2));
            Assert.That(items[0].Element("SubItem0")?.Value, Is.EqualTo("File1"));
            Assert.That(items[0].Element("SubItem1")?.Value, Is.EqualTo("100"));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
