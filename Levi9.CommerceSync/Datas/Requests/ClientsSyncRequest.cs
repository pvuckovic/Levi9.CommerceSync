using Levi9.CommerceSync.Datas.Responses;

namespace Levi9.CommerceSync.Datas.Requests
{
    public class ClientsSyncRequest
    {
        public List<ClientSyncRequest> Clients { get; set; }
        public string? LastUpdate { get; set; }
    }
}
