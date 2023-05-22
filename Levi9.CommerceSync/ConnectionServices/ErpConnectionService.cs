using AutoMapper;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Domain.Model;
using Levi9.CommerceSync.Domain.Repositories;

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
                return new SyncResult<bool> { IsSuccess = false, Message = "Last update not found." };
            }

            var products = await _erpConnection.GetLatestProductsFromErp(lastUpdate);
            if (products.Result == null)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = products.Message };
            } else if(products.Result.Count == 0)
            {
                return new SyncResult<bool> { IsSuccess = false, Message = "There are no products to sync." };
            }

            var mappedProducts = _mapper.Map<List<ProductSyncRequest>>(products.Result);
            var isSynced = await _posConnectionService.SyncProducts(mappedProducts);
            if (isSynced.IsSuccess)
            {
                return new SyncResult<bool> { IsSuccess = true, Message = isSynced.Message };
            }

            return new SyncResult<bool> { IsSuccess = false, Message = isSynced.Message };
        }
    }
}