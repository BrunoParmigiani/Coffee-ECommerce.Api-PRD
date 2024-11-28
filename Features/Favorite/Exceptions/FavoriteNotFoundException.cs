namespace Coffee_Ecommerce.API.Features.Favorite.Exceptions
{
    public sealed class FavoriteNotFoundException : ApplicationException
    {
        public FavoriteNotFoundException(string? message) : base(message)
        { }
    }
}
