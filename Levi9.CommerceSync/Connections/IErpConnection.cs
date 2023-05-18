using Levi9.CommerceSync.Datas.Responses;

namespace Levi9.CommerceSync.Connection
{
    public interface IErpConnection
    {
        Task<List<ProductResponse>> GetLatestProductsFromErp(string number);
    }
}
