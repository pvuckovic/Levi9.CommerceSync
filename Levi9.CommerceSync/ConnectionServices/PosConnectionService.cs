using Levi9.CommerceSync.Connections;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;
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
                return await HandleProductsSyncSuccess(newLastUpdate.Result);
            }
            else
            {
                return new SyncResult<bool> { IsSuccess = false, Message = newLastUpdate.Message };
            }
        }

        private async Task<SyncResult<bool>> HandleProductsSyncSuccess(string updatedLastUpdate)
        {
            var isUpdated = await _syncRepository.UpdateLastUpdate("PRODUCT", updatedLastUpdate);
            if (isUpdated.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = true, Message = "SYNC: Products synchronized successfully." };
            }
            else
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Failed to synchronize products." };
            }
        }


        public async Task<SyncResult<ClientSyncResponse>> SyncClients(List<ClientResponse> erpClients, string lastUpdate)
        {
            ClientSyncRequest syncRequest = new ClientSyncRequest{ LastUpdate = lastUpdate, Clients = erpClients};
            var posClients = await _posConnection.GetLatestClientsFromPos(syncRequest);
            return new SyncResult<ClientSyncResponse> {  IsSuccess = true, Message = posClients.Message, Result = posClients.Result };
        }
    }
}
