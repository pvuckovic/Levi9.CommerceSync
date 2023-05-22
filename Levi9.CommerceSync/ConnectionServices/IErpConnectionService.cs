using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync
{
    public interface IErpConnectionService
    {
        Task<SyncResult> SyncProducts();
    }
}
