using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;
using Levi9.CommerceSync.Domain.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Levi9.CommerceSync.Connections
{
    public class PosConnection : IPosConnection
    {

        public async Task<SyncResult<string>> UpsertProducts(List<ProductSyncRequest> products)
        {
                var options = new RestClientOptions("http://localhost:5067");
                var client = new RestClient(options);

                var request = new RestRequest("/v1/Product/sync", Method.Post);
                request.AddJsonBody(products);

                var response = await client.ExecuteAsync(request);


                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<string>(response.Content);
                    return new SyncResult<string> { IsSuccess = true, Result = result, Message = "POS: Products updated successfully." };
                }
                else
                {
                    return new SyncResult<string> { IsSuccess = false, Result = null, Message = "POS: " + response.ErrorMessage };
                }
        }

        public async Task<SyncResult<ClientSyncResponse>> GetLatestClientsFromPos(ClientSyncRequest syncRequest)
        {
            var options = new RestClientOptions("http://localhost:5067");
            var client = new RestClient(options);
            var request = new RestRequest("/v1/Client/sync/", Method.Post);
            request.AddJsonBody(syncRequest);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                ClientSyncResponse result = JsonConvert.DeserializeObject<ClientSyncResponse>(response.Content);
                return new SyncResult<ClientSyncResponse> { IsSuccess = true, Result = result, Message = "POS: Clients retrieved successfully." };
            }
            else
            {
                return new SyncResult<ClientSyncResponse> { IsSuccess = false, Result = null, Message = "POS: " + response.ErrorMessage };
            }
        }
    }
}
