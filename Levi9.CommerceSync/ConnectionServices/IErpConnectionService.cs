namespace Levi9.CommerceSync
{
    public interface IErpConnectionService
    {
        Task<bool> SyncProducts();
    }
}
