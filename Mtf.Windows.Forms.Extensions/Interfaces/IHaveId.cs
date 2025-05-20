namespace Mtf.Windows.Forms.Extensions.Interfaces
{
    public interface IHaveId<TIdType>
        where TIdType : struct
    {
        TIdType Id { get; }
    }
}
