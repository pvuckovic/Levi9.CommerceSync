using Levi9.CommerceSync.Datas.Requests;

namespace Levi9.CommerceSync.Connections
{
    public interface IPosConnection
    {
        Task<string> UpsertProducts(List<ProductSyncRequest> products);
    }
}
