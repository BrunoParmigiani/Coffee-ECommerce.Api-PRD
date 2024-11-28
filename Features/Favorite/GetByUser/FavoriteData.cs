using Coffee_Ecommerce.API.Features.Product;

namespace Coffee_Ecommerce.API.Features.Favorite.GetByUser
{
    public sealed class FavoriteData
    {
        public Guid FavoriteId { get; set; }
        public ProductEntity Product { get; set; }
    }
}
