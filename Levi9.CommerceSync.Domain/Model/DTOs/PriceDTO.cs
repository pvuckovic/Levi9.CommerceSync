namespace Levi9.CommerceSync.Domain.Model.DTOs
{
    public class PriceDTO
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public float PriceValue { get; set; }
        public string Currency { get; set; }
        public string LastUpdate { get; set; }
    }
}
