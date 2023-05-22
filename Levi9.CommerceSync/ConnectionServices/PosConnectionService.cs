using Levi9.CommerceSync.Connections;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Domain.Model;
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

        public async Task<SyncResult<bool>> SyncProducts(List<ProductSyncRequest> products)
        {
            var newLastUpdate = await _posConnection.UpsertProducts(products);
            if (newLastUpdate.IsSuccess)
            {
                var isUpdated = await _syncRepository.UpdateLastUpdate("PRODUCT", newLastUpdate.Result);
                if (isUpdated)
                {
                    return new SyncResult<bool> { IsSuccess = true, Message = "Products synchronized successfully." };
                }
                else
                {
                    return new SyncResult<bool> { IsSuccess = false, Message = "Failed to synchronize products." };
                }
            }
            else
            {
                return new SyncResult<bool> { IsSuccess = false, Message = newLastUpdate.Message };
            }
        }
    }
}
