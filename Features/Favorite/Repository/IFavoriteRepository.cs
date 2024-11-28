using Coffee_Ecommerce.API.Features.Favorite;

namespace Coffee_Ecommerce.API.Features.Favorite.Repository
{
    public interface IFavoriteRepository
    {
        public Task<FavoriteEntity> CreateAsync(FavoriteEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<FavoriteEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}