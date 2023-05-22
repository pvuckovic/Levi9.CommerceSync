using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.Connection
{
    public interface IErpConnection
    {
        Task<SyncResult<List<ProductResponse>>> GetLatestProductsFromErp(string number);
    }
}
