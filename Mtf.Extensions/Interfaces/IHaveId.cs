namespace Mtf.Extensions.Interfaces
{
    public interface IHaveId<TIdType>
        where TIdType : struct
    {
        TIdType Id { get; }
    }
}
