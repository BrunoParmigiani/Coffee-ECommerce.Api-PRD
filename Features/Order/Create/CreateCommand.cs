namespace Coffee_Ecommerce.API.Features.Order.Create
{
    public sealed class CreateCommand
    {
        public Guid UserId { get; set; }
        public int PaymentMethod { get; set; }
        public Guid EstablishmentId { get; set; }
        public int DeliveryTime { get; set; }
        public OrderItem[] Items { get; set; }
        public bool DeniedOrder { get; set; }
        public string? DeniedReason { get; set; }

        public string GetSerializedItems()
        {
            var result = Items.Select(item => item.ToString());

            return string.Join(";", result);
        }

        public float GetTotalValue()
        {
            float sum = Items.Sum(item => item.GetTotalValue());

            return sum;
        }
    }
}
