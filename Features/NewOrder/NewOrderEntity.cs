using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee_Ecommerce.API.Features.NewOrder
{
	[Table("NewOrders")]
	public sealed class NewOrderEntity
	{
		[Column("Id")]
        public Guid Id { get; set; }

		[Column("UserId")]
		public Guid UserId { get; set; }
		
		[Column("PaymentMethod")]
		public int PaymentMethod { get; set; }
		
		[Column("EstablishmentId")]
		public Guid EstablishmentId { get; set; }
		
		[Column("DeliveryTime")]
		public TimeSpan DeliveryTime { get; set; }
		
		[Column("Items")]
		public string Items { get; set; }
		
		[Column("DeniedOrder")]
		public bool DeniedOrder { get; set; }
		
		[Column("UserName")]
		public string UserName { get; set; }
		
		[Column("UserAddress")]
		public string UserAddress { get; set; }
		
		[Column("UserComplement")]
		public string UserComplement { get; set; }
		
		[Column("DeniedReason")]
		public string? DeniedReason { get; set; }
    }
}
