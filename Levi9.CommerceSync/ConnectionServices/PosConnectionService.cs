using AutoMapper;
using Levi9.CommerceSync.Connection;
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
        private readonly IErpConnection _erpConnection;
        private readonly ISyncRepository _syncRepository;
        private readonly IMapper _mapper;

        public PosConnectionService(IPosConnection posConnection, ISyncRepository syncRepository, IMapper mapper, IErpConnection erpConnection)
        {
            _posConnection = posConnection;
            _syncRepository = syncRepository;
            _mapper = mapper;
            _erpConnection = erpConnection;
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


        public async Task<SyncResult<ClientSyncResponse>> SyncClients(List<ClientSyncRequest> erpClients, string lastUpdate)
        {
            ClientsSyncRequest syncRequest = new ClientsSyncRequest{ LastUpdate = lastUpdate, Clients = erpClients };
            var posClients = await _posConnection.UpdateAndRetriveClients(syncRequest);
            return new SyncResult<ClientSyncResponse> {  IsSuccess = true, Message = posClients.Message, Result = posClients.Result };
        }

        public async Task<SyncResult<bool>> SyncDocuments()
        {
            var lastUpdate = await _syncRepository.GetLastUpdate("DOCUMENT");
            if (lastUpdate == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Last update not found." };
            }

            var documents = await _posConnection.GetLatestDocumentsFromPos(lastUpdate);
            if (documents.Result == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = documents.Message };
            }
            else if (documents.Result.Count == 0)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: There are no documents to sync." };
            }

            var mappedDocuments = _mapper.Map<List<DocumentSyncRequest>>(documents.Result);
            var newLastUpdate = await _erpConnection.UpsertDocuments(mappedDocuments);
            if (newLastUpdate.IsSuccess)
            {
                return await HandleDocumentsSyncSuccess(newLastUpdate.Result);
            }
            else
            {
                return new SyncResult<bool> { IsSuccess = false, Message = newLastUpdate.Message };
            }

        }

        private async Task<SyncResult<bool>> HandleDocumentsSyncSuccess(string updatedLastUpdate)
        {
            var isUpdated = await _syncRepository.UpdateLastUpdate("DOCUMENT", updatedLastUpdate);
            if (isUpdated.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = true, Message = "SYNC: Documents synchronized successfully." };
            }
            else
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Failed to synchronize documents." };
            }
        }
    }
}
