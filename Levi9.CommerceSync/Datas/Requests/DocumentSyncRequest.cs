using Levi9.CommerceSync.Datas.Responses;

namespace Levi9.CommerceSync.Datas.Requests
{
    public class DocumentSyncRequest
    {
        public Guid GlobalId { get; set; }
        public Guid ClientId { get; set; }
        public string DocumentType { get; set; }
        public List<DocumentItemSyncResponse> Items { get; set; }
    }
}
