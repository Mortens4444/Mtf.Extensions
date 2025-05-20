using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mtf.Extensions
{
    public static class ComboBoxExtensions
    {
        public static void SelectFirst(this ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        public static void SelectFirstOrSetDisabled(this ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
            else
            {
                comboBox.Enabled = false;
            }
        }

        public static void AddItems(this ComboBox comboBox, IEnumerable<object> items)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(items.ToArray());
        }

        public static void AddItemsAndSelectFirst(this ComboBox comboBox, IEnumerable<object> items)
        {
            comboBox.AddItems(items);
            comboBox.SelectFirst();
        }

        ///// <summary>
        ///// Get all items from an enumeration
        ///// </summary>
        ///// <param name="comboBox">The combobox</param>
        ///// <param name="enumType">typeof(enum), enum::typeid</param>
        //public static void GetItems(this ComboBox comboBox, Type enumType)
        //{
        //    Utils.GetItems(comboBox, enumType);
        //}

        //public static void GetCOMPorts(this ComboBox comboBox)
        //{
        //    var portNames = SerialPort.GetPortNames();
        //    for (var i = 0; i < portNames.Length; i++)
        //    {
        //        comboBox.Items.Add(portNames[i]);
        //    }

        //    Utils.SelectFirstOrSetDisabled(comboBox);
        //}

        public static int IndexOf(this ComboBox comboBox, object obj)
        {
            return comboBox?.Items.IndexOf(obj) ?? throw new ArgumentNullException(nameof(comboBox));
        }
    }
}
