namespace Coffee_Ecommerce.API.Features.Order
{
    public sealed class OrderItem
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

        public float GetTotalValue()
        {
            return Price * Quantity;
        }

        public override string ToString()
        {
            string total = GetTotalValue().ToString("N2");
            return $"({Quantity}x) {Name} - R${total}";
        }
    }
}