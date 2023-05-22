using Levi9.CommerceSync.Connections;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Requests;
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
        private PosConnectionService _posConnectionService;

        [SetUp]
        public void Setup()
        {
            _posConnectionMock = new Mock<IPosConnection>();
            _syncRepositoryMock = new Mock<ISyncRepository>();
            _posConnectionService = new PosConnectionService(_posConnectionMock.Object, _syncRepositoryMock.Object);
        }

        [Test]
        public async Task SyncProducts_WithSuccessfulSync_ReturnsSuccessResult()
        {
            // Arrange
            var products = new List<ProductSyncRequest>();
            var newLastUpdate = new SyncResult<string> { IsSuccess = true, Result = "123212343256785436" };
            _posConnectionMock.Setup(p => p.UpsertProducts(products)).ReturnsAsync(newLastUpdate);
            _syncRepositoryMock.Setup(s => s.UpdateLastUpdate("PRODUCT", "123212343256785436")).ReturnsAsync(true);
            // Act
            var result = await _posConnectionService.SyncProducts(products);
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Products synchronized successfully.", result.Message);
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
            _syncRepositoryMock.Setup(s => s.UpdateLastUpdate("PRODUCT", "123212343256785436")).ReturnsAsync(false);
            // Act
            var result = await _posConnectionService.SyncProducts(products);
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Failed to synchronize products.", result.Message);
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
    }
}
