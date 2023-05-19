using Levi9.CommerceSync.Connections;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Domain.Repositories;

namespace Levi9.CommerceSync.ConnectionServices
{
    public class PosConnectionService : IPosConnectionService
    {

        private readonly IPosConnection _posConnection;
        private readonly ISyncRepository _syncRepository;

        public PosConnectionService(IPosConnection posConnection, ISyncRepository syncRepository)
        {
            _posConnection = posConnection;
            _syncRepository = syncRepository;
        }

        public async Task<bool> SyncProducts(List<ProductSyncRequest> products)
        {
            var newLastUpdate = _posConnection.UpsertProducts(products);
            if(newLastUpdate.Result == null)
            {
                return false;
            }
            var isUpdated = await _syncRepository.UpdateLastUpdate("PRODUCT", newLastUpdate.Result);
            return isUpdated;
        }
    }
}
