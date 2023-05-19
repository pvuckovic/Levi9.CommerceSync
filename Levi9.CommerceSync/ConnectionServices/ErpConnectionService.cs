using AutoMapper;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Requests;
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
        public async Task<bool> SyncProducts()
        {
            var lastUpdate = _syncRepository.GetLastUpdate("PRODUCT").Result;
            if(lastUpdate == null)
            {
                return false;
            }
            var products = await _erpConnection.GetLatestProductsFromErp(lastUpdate);
            if(products == null)
            {
                return false;
            }
            var mappedProducts = _mapper.Map<List<ProductSyncRequest>>(products);
            var isSynced =  await _posConnectionService.SyncProducts(mappedProducts);
            if(isSynced)
            {
                return true;
            }
            return false;
        }
    }
}