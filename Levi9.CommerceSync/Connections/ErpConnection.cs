﻿using Levi9.CommerceSync.Datas.Responses;
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

        public async Task<SyncResult<List<ClientResponse>>> GetLatestClientsFromErp(string lastUpdate)
        {
            var options = new RestClientOptions("http://localhost:5091");
            var client = new RestClient(options);
            var request = new RestRequest("/v1/Client/sync/" + lastUpdate, Method.Get);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                List<ClientResponse> result = JsonConvert.DeserializeObject<List<ClientResponse>>(response.Content);
                return new SyncResult<List<ClientResponse>> { IsSuccess = true, Result = result, Message = "ERP: Clients retrieved successfully." };
            }
            else
            {
                return new SyncResult<List<ClientResponse>> { IsSuccess = false, Message = "ERP: " + response.ErrorMessage };
            }
        }

        public async Task<SyncResult<string>> SyncClientsOnErp(List<ClientResponse> erpClients)
        {
            var options = new RestClientOptions("http://localhost:5091");
            var client = new RestClient(options);
            var request = new RestRequest("/v1/Client/sync/", Method.Post);
            request.AddJsonBody(request);
            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<string>(response.Content);
                return new SyncResult<string> { IsSuccess = true, Result = result, Message = "ERP: Clients updated successfully." };
            }
            else
            {
                return new SyncResult<string> { IsSuccess = false, Message = "ERP: " + response.ErrorMessage };
            }
        }

    }
}
