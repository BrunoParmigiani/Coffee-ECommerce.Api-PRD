namespace Coffee_Ecommerce.API.Features.Establishment.Exceptions
{
    public sealed class ValuesAlreadyInUseException : ApplicationException
    {
        public ValuesAlreadyInUseException(string? message) : base(message)
        { }
    }
}
