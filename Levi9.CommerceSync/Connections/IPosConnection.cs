using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.Connections
{
    public interface IPosConnection
    {
        Task<SyncResult<string>> UpsertProducts(List<ProductSyncRequest> products);
    }
}
