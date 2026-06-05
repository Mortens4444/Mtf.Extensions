using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Utils;

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

        public static void FillAndSelect(this ComboBox comboBox, IEnumerable list, int selectedIndex = 0)
        {
            comboBox.DataSource = list;
            SafeSelect(comboBox, selectedIndex);
        }

        public static void FillAndSelect<T>(this ComboBox comboBox, IList<T> list, int selectedIndex = 0)
        {
            comboBox.DataSource = list;
            SafeSelect(comboBox, selectedIndex);
        }

        public static void FillAndSelectFirst<T>(this ComboBox comboBox, IEnumerable list)
        {
            FillAndSelect(comboBox, list, 0);
        }

        public static void FillAndSelectFirst<T>(this ComboBox comboBox, IList<T> list)
        {
            FillAndSelect(comboBox, list, 0);
        }

        public static void FillAndSelectLast<T>(this ComboBox comboBox, IList<T> list)
        {
            FillAndSelect(comboBox, list, list.Count - 1);
        }

        //public static void FillWithTypesInNamespace(this ComboBox comboBox, Assembly assembly, string @namespace)
        //{
        //    var types = assembly.GetTypesInNamespace(@namespace);
        //    comboBox.DataSource = types.Select(type => new ComboBoxItem(Activator.CreateInstance(type)!)).ToList();
        //    SafeSelect(comboBox, 0);
        //}
        public static void FillWithTypesInNamespace(this ComboBox comboBox, Assembly assembly, string @namespace)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.Namespace == @namespace);

            comboBox.DataSource = types
                .Select(type => new ComboBoxItem(
                    Activator.CreateInstance(type)))
                .ToList();

            SafeSelect(comboBox, 0);
        }

        public static void SafeSelect(this ComboBox comboBox, int selectedIndex)
        {
            if (comboBox.Items.Count > selectedIndex)
            {
                comboBox.SelectedIndex = selectedIndex;
            }
        }

        public static object GetSelectedItemThreadSafe(this ComboBox comboBox)
        {
            try
            {
                return comboBox.Invoke(new Func<object>(() => comboBox.SelectedItem));
            }
            catch
            {
                return null;
            }
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
