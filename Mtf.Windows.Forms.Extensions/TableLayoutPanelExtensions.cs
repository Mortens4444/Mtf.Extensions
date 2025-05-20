using Mtf.Windows.Forms.Extensions.Interfaces;
using System.Windows.Forms;

namespace Mtf.Extensions
{
    public static class TableLayoutPanelExtensions
    {
        public static void SetRowsAndColumns(this TableLayoutPanel tableLayoutPanel, int rows, int columns)
        {
            if (tableLayoutPanel == null)
            {
                return;
            }

            tableLayoutPanel.RowCount = rows;
            tableLayoutPanel.ColumnCount = columns;
            tableLayoutPanel.SetEqualRowsAndColumns();
        }

        public static void SetEqualRowsAndColumns(this TableLayoutPanel tableLayoutPanel)
        {
            if (tableLayoutPanel == null)
            {
                return;
            }

            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();

            for (var i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                var percent = 100f / tableLayoutPanel.RowCount;
                _ = tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, percent));
            }
            for (var i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                var percent = 100f / tableLayoutPanel.ColumnCount;
                _ = tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
            }
        }

        public static void AddControl(this TableLayoutPanel tableLayoutPanel, Control control, IGridPosition gridPosition)
        {
            if (tableLayoutPanel == null || gridPosition == null)
            {
                return;
            }

            tableLayoutPanel.Controls.Add(control, gridPosition.Column, gridPosition.Row);
            tableLayoutPanel.SetColumnSpan(control, gridPosition.ColumnSpan);
            tableLayoutPanel.SetRowSpan(control, gridPosition.RowSpan);
        }
    }
}
