namespace Coffee_Ecommerce.API.Features.Order.Exceptions
{
    public sealed class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException(string? message) : base(message)
        { }
    }
}
