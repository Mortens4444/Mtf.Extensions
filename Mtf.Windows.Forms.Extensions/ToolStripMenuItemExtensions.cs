using System;
using System.Linq;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions
{
    public static class ToolStripMenuItemExtensions
    {
        public static void FillWithEnum<T>(this ToolStripMenuItem menuItem, Action<ToolStripMenuItem, T> onItemClick = null) where T : Enum
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException(nameof(menuItem));
            }

            menuItem.DropDownItems.Clear();

            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var item = new ToolStripMenuItem(value.ToString())
                {
                    Tag = value
                };

                if (onItemClick != null)
                {
                    item.Click += (sender, e) => onItemClick(item, value);
                }

                menuItem.DropDownItems.Add(item);
            }
        }

        public static void FillWithItems<T>(this ToolStripMenuItem menuItem, T[] items, Action<ToolStripMenuItem, T> onItemClick = null)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException(nameof(menuItem));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            menuItem.DropDownItems.Clear();

            foreach (var value in items)
            {
                var item = new ToolStripMenuItem(value.ToString())
                {
                    Tag = value
                };

                if (onItemClick != null)
                {
                    item.Click += (sender, e) => onItemClick(item, value);
                }

                menuItem.DropDownItems.Add(item);
            }
        }
    }
}
