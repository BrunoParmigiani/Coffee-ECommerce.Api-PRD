namespace Coffee_Ecommerce.API.Features.NewOrder.Exceptions
{
    public sealed class NewOrderNotFoundException : ApplicationException
    {
        public NewOrderNotFoundException(string? message) : base(message)
        {
        }
    }
}
