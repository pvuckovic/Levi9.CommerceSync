namespace Levi9.CommerceSync.Datas.Responses
{
    public class ClientSyncResponse
    {
        public List<ClientSyncRequest> Clients { get; set; }
        public string? LastUpdate { get; set; }
    }
}
