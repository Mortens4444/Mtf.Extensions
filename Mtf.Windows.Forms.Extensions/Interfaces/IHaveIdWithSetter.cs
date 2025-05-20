namespace Mtf.Windows.Forms.Extensions.Interfaces
{
    public interface IHaveIdWithSetter<TIdType>
        where TIdType : struct
    {
        TIdType Id { get; set; }
    }
}
