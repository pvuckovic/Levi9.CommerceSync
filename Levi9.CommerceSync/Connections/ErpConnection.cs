using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Levi9.CommerceSync.Connection
{
    public class ErpConnection : IErpConnection
    {
        public async Task<SyncResult<List<ProductResponse>>> GetLatestProductsFromErp(string lastUpdate)
        {
                var options = new RestClientOptions("http://localhost:5091");
                var client = new RestClient(options);
                var request = new RestRequest("/v1/Product/sync/" + lastUpdate, Method.Get);
                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                List<ProductResponse> result = JsonConvert.DeserializeObject<List<ProductResponse>>(response.Content);
                return new SyncResult<List<ProductResponse>> { IsSuccess = true, Result = result, Message = "ERP: Products retrieved successfully." };
                }
                else
                {
                    return new SyncResult<List<ProductResponse>> { IsSuccess = false, Message = "ERP: " + response.ErrorMessage };
                }
        }

        private async Task<string> Login()
        {
            var authenticationRequest = new
            {
                email = "user@example.com",
                password = "string1!",
            };

            var client = new RestClient("http://localhost:5091");
            var request = new RestRequest("/v1/Authentication", Method.Post);
            request.AddJsonBody(authenticationRequest);

            var response = await client.ExecuteAsync(request);

            var result = JsonConvert.DeserializeObject<string>(response.Content);
            return result;
        }
    }
}
