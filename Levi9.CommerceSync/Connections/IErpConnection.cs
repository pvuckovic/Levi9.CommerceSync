using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.Connection
{
    public interface IErpConnection
    {
        Task<SyncResult<List<ProductResponse>>> GetLatestProductsFromErp(string lastUpdate);
        Task<SyncResult<List<ClientResponse>>> GetLatestClientsFromErp(string lastUpdate);
        Task<SyncResult<string>> SyncClientsOnErp(List<ClientResponse> erpClients);
    }
}
