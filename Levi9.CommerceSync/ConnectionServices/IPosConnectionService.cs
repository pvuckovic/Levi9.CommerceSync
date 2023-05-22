using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.ConnectionServices
{
    public interface IPosConnectionService
    {
        Task<SyncResult> SyncProducts(List<ProductSyncRequest> products);
    }
}
