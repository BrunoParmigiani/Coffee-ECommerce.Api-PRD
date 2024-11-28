namespace Coffee_Ecommerce.API.Features.Order.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public float TotalValue { get; set; }
        public int PaymentMethod { get; set; }
        public bool Paid { get; set; }
        public bool Delivered { get; set; }
        public Guid EstablishmentId { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public DateTime DeliveredAtTime { get; set; }
        public DateTime OrderedAt { get; set; }
        public string[] Items { get; set; }
        public bool Rated { get; set; } = default;
        public int? TimeRating { get; set; } = default;
        public int? QualityRating { get; set; } = default;
        public string? UserComments { get; set; } = default;
        public bool DeniedOrder { get; set; } = default;
        public string? DeniedReason { get; set; } = default;
    }
}
