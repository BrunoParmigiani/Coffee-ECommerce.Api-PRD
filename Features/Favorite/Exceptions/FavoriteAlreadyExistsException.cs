namespace Coffee_Ecommerce.API.Features.Favorite.Exceptions
{
    public sealed class FavoriteAlreadyExistsException : ApplicationException
    {
        public FavoriteAlreadyExistsException(string? message) : base(message)
        { }
    }
}
