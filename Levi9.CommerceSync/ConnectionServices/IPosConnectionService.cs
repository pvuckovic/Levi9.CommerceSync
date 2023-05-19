using Levi9.CommerceSync.Datas.Requests;

namespace Levi9.CommerceSync.ConnectionServices
{
    public interface IPosConnectionService
    {
        Task<bool> SyncProducts(List<ProductSyncRequest> products);
    }
}
