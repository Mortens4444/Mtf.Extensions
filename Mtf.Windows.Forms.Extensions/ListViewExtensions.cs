using Mtf.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class ListViewExtensions
    {
        public const string Directory = "DIR";

        public const string ParentDirectory = "..";

        private static readonly Comparison<ListViewItem> Comparer = new Comparison<ListViewItem>((item1, item2) => { return String.Compare(item1.Text, item2.Text); });

        public static void PopulateWithLegoMindstormEV3FolderContent(this ListView listView, IEnumerable<string> folderContent, bool addLinkToParentDirectory)
        {
            listView.Items.Clear();

            ListViewItem listViewItem;
            if (addLinkToParentDirectory)
            {
                listViewItem = new ListViewItem
                {
                    Text = ParentDirectory
                };
                listViewItem.SubItems.Add(Directory);
                listView.Items.Add(listViewItem);
            }

            var items = new List<ListViewItem>();
            foreach (var item in folderContent)
            {
                listViewItem = new ListViewItem();
                if (item != "../")
                {
                    if (item.EndsWith("/"))
                    {
                        listViewItem.Text = item.TrimEnd('/');
                        listViewItem.SubItems.Add(Directory);
                    }
                    else
                    {
                        var properties = item.Split(' ');
                        if (properties.Length > 2)
                        {
                            var fileSize = Int32.Parse(properties[1], NumberStyles.HexNumber);
                            listViewItem.Text = String.Join(" ", properties.Skip(2));
                            listViewItem.SubItems.Add(String.Empty);
                            listViewItem.SubItems.Add(fileSize.ToString());
                            listViewItem.SubItems.Add(properties[0]);
                        }
                    }
                    items.Add(listViewItem);
                }
            }
            items.Sort(Comparer);
            for (int i = 0; i < items.Count; i++)
            {
                if (i % 2 == 0)
                {
                    items[i].BackColor = Color.LightBlue;
                }
            }
            listView.Items.AddRange(items.ToArray());
        }

        public static bool HasElementWithGuid(this ListView listView, string guid)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Tag is IHaveGuid guidOwner && guidOwner.Guid == guid)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasElementWithId(this ListView listView, long id)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Tag is IHaveId<long> idOwner && idOwner.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasElementWithTag(this ListView listView, object value)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Tag.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Any(this ListView.ListViewItemCollection items, Func<ListViewItem, bool> predicate)
        {
            foreach (ListViewItem item in items)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static void SelectAll(this ListView.ListViewItemCollection items)
        {
            foreach (ListViewItem item in items)
            {
                item.Selected = true;
            }
        }

        public static void SelectAll(this ListViewGroup group)
        {
            foreach (ListViewItem item in group.Items)
            {
                item.Selected = true;
            }
        }

        public static void AddItems<T>(this ListView listView, IEnumerable<T> items, Func<T, ListViewItem> itemConverter, bool clearItems = true)
        {
            if (clearItems)
            {
                listView.Items.Clear();
            }

            var lvItems = new List<ListViewItem>();
            foreach (var item in items)
            {
                lvItems.Add(itemConverter(item));
            }

            listView.Items.AddRange(lvItems.ToArray());
        }
    }
}
