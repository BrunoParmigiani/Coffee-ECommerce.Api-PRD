using Coffee_Ecommerce.API.Features.Order;

namespace Coffee_Ecommerce.API.Features.NewOrder.DTO
{
    public sealed class NewOrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int PaymentMethod { get; set; }
        public Guid EstablishmentId { get; set; }
        public int DeliveryTime { get; set; }
        public OrderItem[] Items { get; set; }
        public bool DeniedOrder { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserComplement { get; set; }
        public string? DeniedReason { get; set; }
    }
}
