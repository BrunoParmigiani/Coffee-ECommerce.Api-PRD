using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Product.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Features.Product.Repository
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly PostgreContext _context;

        public ProductRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<ProductEntity> CreateAsync(ProductEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var establishmentExists = await _context.Establishments.AnyAsync(establishment => establishment.Id == entity.EstablishmentId);

            if (!establishmentExists)
                throw new EstablishmentNotFoundException("Establishment not found");

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
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (ProductNotFoundException ex)
            {
                throw new ProductNotFoundException(ex.Message);
            }
        }

        public async Task<List<ProductEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }

        public async Task<List<ProductEntity>> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var establishmentExists = await _context.Establishments.AnyAsync(establishment => establishment.Id == id);

            if (!establishmentExists)
                throw new EstablishmentNotFoundException("Establishment not found");

            return await _context.Products
                .Where(product => product.EstablishmentId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Products.SingleOrDefaultAsync(product => product.Id == id, cancellationToken);

            if (result == null)
                throw new ProductNotFoundException("Product not found");

            return result;
        }

        public async Task<ProductEntity> UpdateAsync(ProductEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Product cannot be null");

            var establishmentExists = await _context.Establishments.AnyAsync(establishment => establishment.Id == entity.EstablishmentId);

            if (!establishmentExists)
                throw new EstablishmentNotFoundException("Establishment not found");

            var product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(product => product.Id == entity.Id, cancellationToken);

            if (product == null)
                throw new ProductNotFoundException("Product not found");

            if (entity.CreatorId != product.CreatorId)
                throw new ArgumentException("Forbidden operation");

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
