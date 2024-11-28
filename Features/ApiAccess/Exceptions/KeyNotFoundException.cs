namespace Coffee_Ecommerce.API.Features.ApiAccess.Exceptions
{
    public sealed class KeyNotFoundException : ApplicationException
    {
        public KeyNotFoundException(string? message) : base(message)
        {
        }
    }
}
