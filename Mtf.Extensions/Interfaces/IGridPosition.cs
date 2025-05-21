namespace Mtf.Extensions.Interfaces
{
    public interface IGridPosition
    {
        int Column { get; }

        int Row { get; }

        int ColumnSpan { get; }

        int RowSpan { get; }
    }
}
