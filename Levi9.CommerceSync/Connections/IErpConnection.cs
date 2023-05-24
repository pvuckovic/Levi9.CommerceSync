using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.Connection
{
    public interface IErpConnection
    {
        Task<SyncResult<List<ProductResponse>>> GetLatestProductsFromErp(string lastUpdate);
        Task<SyncResult<List<ClientSyncRequest>>> GetLatestClientsFromErp(string lastUpdate);
        Task<SyncResult<string>> SyncClientsOnErp(List<ClientSyncRequest> erpClients);
        Task<SyncResult<string>> UpsertDocuments(List<DocumentSyncRequest> documents);
    }
}
