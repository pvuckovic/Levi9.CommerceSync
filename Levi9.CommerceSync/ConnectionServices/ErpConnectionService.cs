using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.Domain.Repositories;

namespace Levi9.CommerceSync
{
    public class ErpConnectionService : IErpConnectionService
    {
        private readonly IErpConnection _erpConnection;
        private readonly ISyncRepository _syncRepository;

        public ErpConnectionService(IErpConnection erpConnection, ISyncRepository syncRepository)
        {
            _erpConnection = erpConnection;
            _syncRepository = syncRepository;
        }
        public async Task<bool> SyncProducts()
        {
            string lastUpdate = _syncRepository.GetLastUpdate("PRODUCT").Result.LastUpdate;
            var products = await _erpConnection.GetLatestProductsFromErp(lastUpdate);
            return true;
        }
    }
}