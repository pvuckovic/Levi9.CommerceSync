using Levi9.CommerceSync.Datas.Responses;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Levi9.CommerceSync.Connection
{
    public class ErpConnection : IErpConnection
    {
        public async Task<List<ProductResponse>> GetLatestProductsFromErp(string lastUpdate)
        {
            string jwtToken = GenerateJwt();

            var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(jwtToken, "Bearer");

            var options = new RestClientOptions("http://localhost:5091")
            {
                Authenticator = authenticator
            };

            var client = new RestClient(options);

            var request = new RestRequest("/v1/Product/" + 1, Method.Get);
            RestResponse response = await client.GetAsync(request);

            ProductResponse respo = JsonConvert.DeserializeObject<ProductResponse>(response.Content);
            List<ProductResponse> result = JsonConvert.DeserializeObject<List<ProductResponse>>(response.Content);
            return result;
        }

        private static string GenerateJwt()
        {
            var securityKey = Encoding.UTF8.GetBytes("some-signing-key-here");
            var symetricKey = new SymmetricSecurityKey(securityKey);
            var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                                            issuer: "https://localhost:7281",
                                            audience: "https://localhost:7281",
                                            expires: DateTime.Now.Add(new TimeSpan(3600)),
                                            signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
