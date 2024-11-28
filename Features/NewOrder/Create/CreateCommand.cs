using Coffee_Ecommerce.API.Features.Order;

namespace Coffee_Ecommerce.API.Features.NewOrder.Create
{
    public sealed class CreateCommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int PaymentMethod { get; set; }
        public Guid EstablishmentId { get; set; }
        public OrderItem[] Items { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserComplement { get; set; }

        public string GetSerializedItems()
        {
            var result = Items.Select(item =>
            {
                return $"{item.Name};{item.Price};{item.Quantity}";
            });

            return string.Join(";;", result);
        }
    }
}
