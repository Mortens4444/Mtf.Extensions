namespace Mtf.Windows.Forms.Extensions.Interfaces
{
    public class IGridPosition
    {
        public int Column { get; internal set; }

        public int Row { get; internal set; }

        public int ColumnSpan { get; internal set; }

        public int RowSpan { get; internal set; }
    }
}
