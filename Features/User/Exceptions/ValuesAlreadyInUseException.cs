namespace Coffee_Ecommerce.API.Features.User.Exceptions
{
    public sealed class ValuesAlreadyInUseException : ApplicationException
    {
        public ValuesAlreadyInUseException(string? message) : base(message)
        { }
    }
}
