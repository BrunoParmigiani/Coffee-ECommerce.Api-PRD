namespace Coffee_Ecommerce.API.Features.Administrator.Exceptions
{
    public sealed class AdministratorNotFoundException : ApplicationException
    {
        public AdministratorNotFoundException(string? message) : base(message)
        { }
    }
}
