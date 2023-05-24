using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.ConnectionServices
{
    public interface IPosConnectionService
    {
       Task<SyncResult<bool>> SyncProducts(List<ProductSyncRequest> products);
       Task<SyncResult<ClientSyncResponse>> SyncClients(List<ClientSyncRequest> erpClients, string lastUpdate);
        Task<SyncResult<bool>> SyncDocuments();

    }
}
