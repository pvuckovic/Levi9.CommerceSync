using AutoMapper;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.Connections;
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
    public class PosConnectionServiceTests
    {
        private Mock<IPosConnection> _posConnectionMock;
        private Mock<ISyncRepository> _syncRepositoryMock;
        private Mock<IErpConnection> _erpConnection;
        private Mock<IMapper> _mockMapper;
        private PosConnectionService _posConnectionService;

        [SetUp]
        public void Setup()
        {
            _posConnectionMock = new Mock<IPosConnection>();
            _syncRepositoryMock = new Mock<ISyncRepository>();
            _posConnectionService = new PosConnectionService(_posConnectionMock.Object, _syncRepositoryMock.Object, _mockMapper.Object, _erpConnection.Object);
        }

        [Test]
        public async Task SyncProducts_WithSuccessfulSync_ReturnsSuccessResult()
        {
            // Arrange
            var products = new List<ProductSyncRequest>();
            var newLastUpdate = new SyncResult<string> { IsSuccess = true, Result = "123212343256785436" };
            _posConnectionMock.Setup(p => p.UpsertProducts(products)).ReturnsAsync(newLastUpdate);
            _syncRepositoryMock.Setup(s => s.UpdateLastUpdate("PRODUCT", "123212343256785436")).ReturnsAsync(new SyncResult<bool> { IsSuccess = true});
            // Act
            var result = await _posConnectionService.SyncProducts(products);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("SYNC: Products synchronized successfully.", result.Message);
        }

        [Test]
        public async Task SyncProducts_WithFailedSync_ReturnsFailedResult()
        {
            // Arrange
            var products = new List<ProductSyncRequest>();
            var newLastUpdate = new SyncResult<string> { IsSuccess = false, Message = "Sync failed." };
            _posConnectionMock.Setup(p => p.UpsertProducts(products)).ReturnsAsync(newLastUpdate);
            // Act
            var result = await _posConnectionService.SyncProducts(products);
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Sync failed.", result.Message);
        }

        [Test]
        public async Task SyncProducts_WithSuccessfulSyncAndFailedUpdate_ReturnsFailedResult()
        {
            // Arrange
            var products = new List<ProductSyncRequest>();
            var newLastUpdate = new SyncResult<string> { IsSuccess = true, Result = "123212343256785436" };
            _posConnectionMock.Setup(p => p.UpsertProducts(products)).ReturnsAsync(newLastUpdate);
            _syncRepositoryMock.Setup(s => s.UpdateLastUpdate("PRODUCT", "123212343256785436")).ReturnsAsync(new SyncResult<bool> { IsSuccess = false });
            // Act
            var result = await _posConnectionService.SyncProducts(products);
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("SYNC: Failed to synchronize products.", result.Message);
        }

        [Test]
        public async Task SyncProducts_WithFailedSyncAndErrorMessage_ReturnsFailedResultWithErrorMessage()
        {
            // Arrange
            var products = new List<ProductSyncRequest>();
            var newLastUpdate = new SyncResult<string> { IsSuccess = false, Message = "Sync failed." };
            _posConnectionMock.Setup(p => p.UpsertProducts(products)).ReturnsAsync(newLastUpdate);
            // Act
            var result = await _posConnectionService.SyncProducts(products);
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Sync failed.", result.Message);
        }

        [Test]
        public async Task SyncClients_ReturnsSyncResultWithSuccess()
        {
            // Arrange
            var erpClients = new List<ClientSyncRequest> {};
            var lastUpdate = "123212343256785436";
            var expectedResult = new ClientSyncResponse {};
            _posConnectionMock.Setup(x => x.UpdateAndRetriveClients(It.IsAny<ClientsSyncRequest>()))
                .ReturnsAsync(new SyncResult<ClientSyncResponse> { IsSuccess = true, Message = "Success", Result = expectedResult });

            // Act
            var result = await _posConnectionService.SyncClients(erpClients, lastUpdate);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Success", result.Message);
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
