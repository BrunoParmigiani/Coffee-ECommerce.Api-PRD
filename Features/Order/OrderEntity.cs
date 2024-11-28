using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.Order
{
    [Table("Orders")]
    public sealed class OrderEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("TotalValue")]
        public float TotalValue { get; set; }

        [Column("PaymentMethod")]
        public int PaymentMethod { get; set; }

        [Column("Paid")]
        public bool Paid { get; set; }

        [Column("Delivered")]
        public bool Delivered { get; set; }

        [Column("EstablishmentId")]
        public Guid EstablishmentId { get; set; }

        [Column("DeliveryTime")]
        public TimeSpan DeliveryTime { get; set; } = default; // hh:mm

        [Column("DeliveredAtTime")]
        public DateTime DeliveredAtTime { get; set; } = default; // dd/MM/yyyy hh:mm

        [Column("OrderedAt")]
        public DateTime OrderedAt { get; set; } = default; // dd/MM/yyyy hh:mm

        [Column("Items")]
        public string Items { get; set; }

        [Column("Rated")]
        public bool Rated { get; set; } = default;

        [Column("TimeRating")]
        public int? TimeRating { get; set; } = default;

        [Column("QualityRating")]
        public int? QualityRating { get; set; } = default;

        [Column("UserComments")]
        public string? UserComments { get; set; } = default;

        [Column("DeniedOrder")]
        public bool DeniedOrder { get; set; } = default;

        [Column("DeniedReason")]
        public string? DeniedReason { get; set; } = default;
    }
}
