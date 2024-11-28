namespace Coffee_Ecommerce.API.Features.Administrator.Exceptions
{
    public sealed class ValuesAlreadyInUseException : ApplicationException
    {
        public ValuesAlreadyInUseException(string? message) : base(message)
        { }
    }
}
