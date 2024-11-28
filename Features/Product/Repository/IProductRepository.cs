namespace Coffee_Ecommerce.API.Features.Product.Repository
{
    public interface IProductRepository
    {
        public Task<ProductEntity> CreateAsync(ProductEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<ProductEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<List<ProductEntity>> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken);
        public Task<ProductEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<ProductEntity> UpdateAsync(ProductEntity entity, CancellationToken cancellationToken);
    }
}
