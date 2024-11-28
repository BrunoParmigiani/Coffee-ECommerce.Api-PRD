namespace Coffee_Ecommerce.API.Features.Order.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }
        public bool Paid { get; set; }
        public bool Delivered { get; set; }
        public int DeliveryTime { get; set; }
    }
}
