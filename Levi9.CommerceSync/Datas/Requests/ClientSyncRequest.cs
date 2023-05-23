using Levi9.CommerceSync.Datas.Responses;

namespace Levi9.CommerceSync.Datas.Requests
{
    public class ClientSyncRequest
    {
        public List<ClientResponse> Clients { get; set; }
        public string? LastUpdate { get; set; }
    }
}
