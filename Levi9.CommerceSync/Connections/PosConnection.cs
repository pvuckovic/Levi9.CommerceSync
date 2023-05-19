using Levi9.CommerceSync.Datas.Requests;
using Newtonsoft.Json;
using RestSharp;

namespace Levi9.CommerceSync.Connections
{
    public class PosConnection : IPosConnection
    {

        public async Task<string> UpsertProducts(List<ProductSyncRequest> products)
        {
            //string jwtToken = Login

            //var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(jwtToken, "Bearer");

            var options = new RestClientOptions("http://localhost:5067")
            {
               // Authenticator = authenticator
            };

            var client = new RestClient(options);

            var request = new RestRequest("/v1/Product/sync", Method.Post);
            request.AddJsonBody(products);

            var response = await client.ExecuteAsync(request);

            var result = JsonConvert.DeserializeObject<string>(response.Content);
            return result;
        }
    }
}
