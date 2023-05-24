namespace Levi9.CommerceSync.Datas.Responses
{
    public class DocumentSyncResponse
    {
        public Guid GlobalId { get; set; }
        public Guid ClientId { get; set; }
        public string DocumentType { get; set; }
        public List<DocumentItemSyncResponse> Items { get; set; }
    }
}
