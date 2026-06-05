using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class ListViewItemExtensions
    {
        public static string ToString(this ListViewItem item)
        {
            return ConvertToString(item);
        }

        public static string ConvertToString(this ListViewItem item)
        {
            if (item == null)
            {
                return String.Empty;
            }

            var toString = new StringBuilder(item.Text);
            for (var i = 1; i < item.SubItems.Count; i++)
            {
                _ = toString.AppendFormat(CultureInfo.InvariantCulture, $"\t{item.SubItems[i].Text}");
            }

            return toString.ToString();
        }

        public static ReadOnlyCollection<string> ToArrayList(this ListViewItem item)
        {
            return new ReadOnlyCollection<string>(ToEnumerable(item).ToList());
        }

        public static string ToStringInPreferredFormat(this ListViewItem item)
        {
            return String.Join("\t", ToEnumerable(item));
        }

        private static IEnumerable<string> ToEnumerable(ListViewItem item)
        {
            return item?.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(subItem => subItem.Text);
        }

        public static bool IsDirectory(this ListViewItem listViewItem)
        {
            return listViewItem.SubItems[1].Text == ListViewExtensions.Directory;
        }

        public static int GetFileSize(this ListViewItem listViewItem)
        {
            return Convert.ToInt32(listViewItem.SubItems[2].Text);
        }

        public static string ChangeWorkingDirectory(this ListViewItem selectedListViewItem, string currentDirectory)
        {
            if (selectedListViewItem.IsDirectory())
            {
                if (selectedListViewItem.Text == ListViewExtensions.ParentDirectory)
                {
                    var index = currentDirectory.LastIndexOf('/');
                    var parentDirectory = index >= 0 ? currentDirectory.Substring(0, index).TrimEnd('/') : currentDirectory;
                    if (parentDirectory == String.Empty)
                    {
                        parentDirectory = "/";
                    }
                    return parentDirectory;
                }
                else
                {
                    var folderSeparator = currentDirectory.EndsWith("/") ? String.Empty : "/";
                    return String.Concat(currentDirectory, folderSeparator, selectedListViewItem.Text);
                }
            }
            return currentDirectory;
        }
    }
}
