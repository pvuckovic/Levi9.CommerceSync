namespace Levi9.CommerceSync.Datas.Responses
{
    public class ClientSyncRequest
    {
        public Guid GlobalId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string LastUpdate { get; set; }
        public string Salt { get; set; }
    }
}
