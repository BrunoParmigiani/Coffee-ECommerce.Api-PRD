namespace Coffee_Ecommerce.API.Features.FooterItem.Exceptions
{
    public sealed class FooterItemNotFoundException : ApplicationException
    {
        public FooterItemNotFoundException(string? message) : base(message)
        { }
    }
}
