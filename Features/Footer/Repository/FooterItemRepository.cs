using Coffee_Ecommerce.API.Features.FooterItem.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Features.FooterItem.Repository
{
    public sealed class FooterItemRepository : IFooterItemRepository
    {
        private readonly PostgreContext _context;

        public FooterItemRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<FooterItemEntity> CreateAsync(FooterItemEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                _context.Remove(result);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (FooterItemNotFoundException ex)
            {
                throw new FooterItemNotFoundException(ex.Message);
            }
        }

        public async Task<List<FooterItemEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.FooterItems.ToListAsync(cancellationToken);
        }

        public async Task<FooterItemEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.FooterItems.SingleOrDefaultAsync(items => items.Id == id);

            if (result == null)
                throw new FooterItemNotFoundException("Item not found");

            return result;
        }

        public async Task<FooterItemEntity> UpdateAsync(FooterItemEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var exists = await _context.FooterItems.AnyAsync(items => items.Id == entity.Id, cancellationToken);

            if (!exists)
                throw new FooterItemNotFoundException("Item not found");

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
