using Levi9.CommerceSync.Datas.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace Levi9.CommerceSync.Connection
{
    public class ErpConnection : IErpConnection
    {
        public async Task<List<ProductResponse>> GetLatestProductsFromErp(string lastUpdate)
        {
            string jwtToken = Login().Result;

            //var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(jwtToken, "Bearer");

            var options = new RestClientOptions("http://localhost:5091")
            {
                //Authenticator = new JwtAuthenticator(jwtToken)
            };

            var client = new RestClient(options);

            var request = new RestRequest("/v1/Product/sync/" + lastUpdate, Method.Get);
            //request.AddHeader("Authorization", "Bearer " + jwtToken);
            //client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", jwtToken));
            RestResponse response = await client.ExecuteAsync(request);

            List<ProductResponse> result = JsonConvert.DeserializeObject<List<ProductResponse>>(response.Content);
            return result;
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
