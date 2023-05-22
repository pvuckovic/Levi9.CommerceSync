using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace Levi9.CommerceSync.UnitTests.Connections
{
    [TestFixture]
    public class ProductSyncTests
    {
        //private Mock<IRestClient> restClientMock;
        //private ProductSync productSync;

        //[SetUp]
        //public void Setup()
        //{
        //    restClientMock = new Mock<IRestClient>();
        //    productSync = new ProductSync(restClientMock.Object);
        //}

        //[Test]
        //public async Task GetLatestProductsFromErp_WithSuccessfulResponse_ReturnsSyncResultWithProducts()
        //{
        //    // Arrange
        //    string lastUpdate = "2023-05-20T12:00:00";
        //    var responseContent = "[{\"Id\": 1, \"Name\": \"Product 1\"}, {\"Id\": 2, \"Name\": \"Product 2\"}]";
        //    var response = new RestResponse
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = responseContent
        //    };
        //    restClientMock.Setup(c => c.ExecuteAsync(It.IsAny<IRestRequest>())).ReturnsAsync(response);

        //    // Act
        //    var syncResult = await productSync.GetLatestProductsFromErp(lastUpdate);

        //    // Assert
        //    Assert.IsTrue(syncResult.IsSuccess);
        //    Assert.AreEqual("ERP: Products retrieved successfully.", syncResult.Message);
        //    Assert.AreEqual(2, syncResult.Result.Count);
        //    Assert.AreEqual(1, syncResult.Result[0].Id);
        //    Assert.AreEqual("Product 1", syncResult.Result[0].Name);
        //    Assert.AreEqual(2, syncResult.Result[1].Id);
        //    Assert.AreEqual("Product 2", syncResult.Result[1].Name);
        //}

        //[Test]
        //public async Task GetLatestProductsFromErp_WithErrorResponse_ReturnsSyncResultWithError()
        //{
        //    // Arrange
        //    string lastUpdate = "2023-05-20T12:00:00";
        //    var errorMessage = "Error message from ERP API";
        //    var response = new RestResponse
        //    {
        //        StatusCode = HttpStatusCode.InternalServerError,
        //        ErrorMessage = errorMessage
        //    };
        //    restClientMock.Setup(c => c.ExecuteAsync(It.IsAny<IRestRequest>())).ReturnsAsync(response);

        //    // Act
        //    var syncResult = await productSync.GetLatestProductsFromErp(lastUpdate);

        //    // Assert
        //    Assert.IsFalse(syncResult.IsSuccess);
        //    Assert.AreEqual("ERP: " + errorMessage, syncResult.Message);
        //    Assert.IsNull(syncResult.Result);
        //}
    }
}
