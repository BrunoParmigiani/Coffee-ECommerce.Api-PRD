namespace Coffee_Ecommerce.API.Features.Establishment.Exceptions
{
    public sealed class EstablishmentNotFoundException : ApplicationException
    {
        public EstablishmentNotFoundException(string? message) : base(message)
        { }
    }
}
