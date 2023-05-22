using AutoMapper;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Requests;
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
        public async Task<SyncResult> SyncProducts()
        {
            var lastUpdate = await _syncRepository.GetLastUpdate("PRODUCT");
            if (lastUpdate == null)
            {
                return new SyncResult { IsSuccess = false, Message = "Last update not found." };
            }

            var products = await _erpConnection.GetLatestProductsFromErp(lastUpdate);
            if (products.IsNullOrEmpty())
            {
                return new SyncResult { IsSuccess = false, Message = "No products to sync." };
            }

            var mappedProducts = _mapper.Map<List<ProductSyncRequest>>(products);
            var isSynced = await _posConnectionService.SyncProducts(mappedProducts);
            if (isSynced.IsSuccess)
            {
                return new SyncResult { IsSuccess = true, Message = "Products synchronized successfully." };
            }

            return new SyncResult { IsSuccess = false, Message = "Failed to sync products." };
        }
    }
}