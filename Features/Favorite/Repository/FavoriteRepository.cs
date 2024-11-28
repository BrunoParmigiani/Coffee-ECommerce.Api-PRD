using Coffee_Ecommerce.API.Features.Favorite.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Features.Favorite.Repository
{
    public sealed class FavoriteRepository : IFavoriteRepository
    {
        private readonly PostgreContext _context;

        public FavoriteRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<FavoriteEntity> CreateAsync(FavoriteEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity cannot be null");

            var exists = await _context.Favorites.AnyAsync(favorite => favorite.UserId == entity.UserId && favorite.ProductId == entity.ProductId, cancellationToken);

            if (exists)
                throw new FavoriteAlreadyExistsException("Registry already exists for this product");

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetById(id, cancellationToken);

                _context.Remove(result);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (FavoriteNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<FavoriteEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _context.Favorites.Where(favorite => favorite.UserId == userId).ToListAsync(cancellationToken);

            return result;
        }

        private async Task<FavoriteEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Favorites.SingleOrDefaultAsync(favorite => favorite.Id == id, cancellationToken);

            if (result == null)
                throw new FavoriteNotFoundException("Favorite not found");

            return result;
        }
    }
}