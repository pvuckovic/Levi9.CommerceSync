using AutoMapper;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;
using Levi9.CommerceSync.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace Levi9.CommerceSync.UnitTests.ConnectionServices
{
    [TestFixture]
    public class ErpConnectionServiceTests
    {
        private ErpConnectionService _erpConnectionService;
        private Mock<IErpConnection> _mockErpConnection;
        private Mock<ISyncRepository> _mockSyncRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IPosConnectionService> _mockPosConnectionService;

        [SetUp]
        public void Setup()
        {
            _mockErpConnection = new Mock<IErpConnection>();
            _mockSyncRepository = new Mock<ISyncRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockPosConnectionService = new Mock<IPosConnectionService>();

            _erpConnectionService = new ErpConnectionService(
                _mockErpConnection.Object,
                _mockSyncRepository.Object,
                _mockMapper.Object,
                _mockPosConnectionService.Object
            );
        }

        [Test]
        public async Task SyncProducts_LastUpdateNotFound_ReturnsSyncResultWithFailure()
        {
            // Arrange
            string errorMessage = "Last update not found.";
            _mockSyncRepository.Setup(repo => repo.GetLastUpdate("PRODUCT")).ReturnsAsync((string)null);

            // Act
            var result = await _erpConnectionService.SyncProducts();

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(errorMessage, result.Message);
        }

        [Test]
        public async Task SyncProducts_NoProductsToSync_ReturnsSyncResultWithFailure()
        {
            // Arrange
            var lastUpdate = "123543765437895643";
            string errorMessage = "There are no products to sync.";
            var products = new SyncResult<List<ProductResponse>> { IsSuccess = true, Result = new List<ProductResponse>() };
            _mockSyncRepository.Setup(repo => repo.GetLastUpdate("PRODUCT")).ReturnsAsync(lastUpdate);
            _mockErpConnection.Setup(conn => conn.GetLatestProductsFromErp(lastUpdate)).ReturnsAsync(products);

            // Act
            var result = await _erpConnectionService.SyncProducts();

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(errorMessage, result.Message);
        }

        [Test]
        public async Task SyncProducts_ProductsSyncedSuccessfully_ReturnsSyncResultWithSuccess()
        {
            // Arrange
            var lastUpdate = "123543765437895643";
            var products = new SyncResult<List<ProductResponse>> { IsSuccess = true, Result = new List<ProductResponse> { new ProductResponse() } };
            var mappedProducts = new List<ProductSyncRequest>();
            var syncResult = new SyncResult<bool> { IsSuccess = true, Message = "Sync successful." };
            _mockSyncRepository.Setup(repo => repo.GetLastUpdate("PRODUCT")).ReturnsAsync(lastUpdate);
            _mockErpConnection.Setup(conn => conn.GetLatestProductsFromErp(lastUpdate)).ReturnsAsync(products);
            _mockMapper.Setup(mapper => mapper.Map<List<ProductSyncRequest>>(products.Result)).Returns(mappedProducts);
            _mockPosConnectionService.Setup(service => service.SyncProducts(mappedProducts)).ReturnsAsync(syncResult);

            // Act
            var result = await _erpConnectionService.SyncProducts();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(syncResult.Message, result.Message);
        }

        [Test]
        public async Task SyncProducts_ProductsSyncFailed_ReturnsSyncResultWithFailure()
        {
            // Arrange
            var lastUpdate = "123543765437895643";
            var products = new SyncResult<List<ProductResponse>> { IsSuccess = true, Result = new List<ProductResponse> { new ProductResponse() } };
            var mappedProducts = new List<ProductSyncRequest> { new ProductSyncRequest { Name = "Shirt" } };
            var syncResult = new SyncResult<bool> { IsSuccess = false, Message = "Sync failed." };
            _mockSyncRepository.Setup(repo => repo.GetLastUpdate("PRODUCT")).ReturnsAsync(lastUpdate);
            _mockErpConnection.Setup(conn => conn.GetLatestProductsFromErp(lastUpdate)).ReturnsAsync(products);
            _mockMapper.Setup(mapper => mapper.Map<List<ProductSyncRequest>>(products.Result)).Returns(mappedProducts);
            _mockPosConnectionService.Setup(service => service.SyncProducts(mappedProducts)).ReturnsAsync(syncResult);

            // Act
            var result = await _erpConnectionService.SyncProducts();

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(syncResult.Message, result.Message);
        }

    }
}
