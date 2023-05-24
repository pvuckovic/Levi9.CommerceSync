using AutoMapper;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;
using Levi9.CommerceSync.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Levi9.CommerceSync
{
    public class ErpConnectionService : IErpConnectionService
    {
        private readonly IErpConnection _erpConnection;
        private readonly ISyncRepository _syncRepository;
        private readonly IMapper _mapper;
        private readonly IPosConnectionService _posConnectionService;

        public ErpConnectionService(IErpConnection erpConnection, ISyncRepository syncRepository, IMapper mapper, IPosConnectionService posConnectionService)
        {
            _erpConnection = erpConnection;
            _syncRepository = syncRepository;
            _mapper = mapper;
            _posConnectionService = posConnectionService;
        }
        public async Task<SyncResult<bool>> SyncProducts()
        {
            var lastUpdate = await _syncRepository.GetLastUpdate("PRODUCT");
            if (lastUpdate == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Last update not found." };
            }

            var products = await _erpConnection.GetLatestProductsFromErp(lastUpdate);
            if (products.Result == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = products.Message };
            } else if(products.Result.Count == 0)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: There are no products to sync." };
            }

            var mappedProducts = _mapper.Map<List<ProductSyncRequest>>(products.Result);
            var isSynced = await _posConnectionService.SyncProducts(mappedProducts);
            if (isSynced.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = true, Message = isSynced.Message };
            }

            return new SyncResult<bool> { IsSuccess = false, Message = isSynced.Message };
        }

        public async Task<SyncResult<bool>> SyncClients()
        {
            var lastUpdate = await _syncRepository.GetLastUpdate("CLIENT");
            if (lastUpdate == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Last update not found." };
            }

            var erpClients = await _erpConnection.GetLatestClientsFromErp(lastUpdate);
            if (erpClients.Result == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = erpClients.Message };
            }

            var posClients = await _posConnectionService.SyncClients(erpClients.Result, lastUpdate);
            if (posClients.Result == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = posClients.Message };
            }
            else if (posClients.Result.Clients.Count == 0)
            {
                return await HandleNoClientsToSyncOnErp(posClients.Result.LastUpdate, erpClients.Result.Count);
            }
            
            var isSynced = await _erpConnection.SyncClientsOnErp(posClients.Result.Clients);
            if (isSynced.Result.IsNullOrEmpty())
            {
                return new SyncResult<bool> { IsSuccess = false, Message = isSynced.Message };
            }
            else
            {
                return await HandleClientsSyncedOnErp(isSynced.Result);
            }
        }

        //public async Task<SyncResult<bool>> SyncDocuments(List<DocumentSyncRequest> documents)
        //{
        //    var newLastUpdate = await _erpConnection.UpsertDocuments(documents);
        //    if (newLastUpdate.IsSuccess)
        //    {
        //        return await HandleDocumentsSyncSuccess(newLastUpdate.Result);
        //    }
        //    else
        //    {
        //        return new SyncResult<bool> { IsSuccess = false, Message = newLastUpdate.Message };
        //    }
        //}

        private async Task<SyncResult<bool>> HandleClientsSyncedOnErp(string updatedLastUpdate)
        {
            var isUpdated = await _syncRepository.UpdateLastUpdate("CLIENT", updatedLastUpdate);
            if (isUpdated.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = true, Message = "SYNC: Clients synchronized successfully." };
            }
            else
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Failed to synchronize clients on ERP." };
            }
        }

        private async Task<SyncResult<bool>> HandleNoClientsToSyncOnErp(string newLastUpdate, int erpClientsCount)
        {
            var isUpdated = await _syncRepository.UpdateLastUpdate("CLIENT", newLastUpdate);
            if (erpClientsCount == 0 && isUpdated.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: There are no clients to sync." };
            }
            else if (isUpdated.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Clients synchronized on POS successfully. There are no clients to sync on ERP." };
            }
            else {
                return new SyncResult<bool> { IsSuccess = false, Message = "SYNC: Failed to synchronize clients." };
            }
        }
    }
}