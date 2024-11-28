namespace Coffee_Ecommerce.API.Features.User.Exceptions
{
    public sealed class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException(string? message) : base(message)
        { }
    }
}
