using Coffee_Ecommerce.API.Features.Favorite.GetByUser;
using Coffee_Ecommerce.API.Features.Favorite.Delete;
using Coffee_Ecommerce.API.Features.Favorite.GetByUser;

namespace Coffee_Ecommerce.API.Features.Favorite.Business
{
    public interface IFavoriteBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetByUserResult> GetByUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}