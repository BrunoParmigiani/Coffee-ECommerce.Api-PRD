namespace Coffee_Ecommerce.API.Features.Product.Exceptions
{
    public sealed class ProductNotFoundException : ApplicationException
    {
        public ProductNotFoundException(string? message) : base(message)
        { }
    }
}
