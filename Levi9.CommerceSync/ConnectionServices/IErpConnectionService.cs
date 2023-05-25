using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync
{
    public interface IErpConnectionService
    {
        Task<SyncResult<bool>> SyncProducts();
        Task<SyncResult<bool>> SyncClients();
    }
}
