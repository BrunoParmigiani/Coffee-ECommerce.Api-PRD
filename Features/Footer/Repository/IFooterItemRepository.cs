namespace Coffee_Ecommerce.API.Features.FooterItem.Repository
{
    public interface IFooterItemRepository
    {
        public Task<FooterItemEntity> CreateAsync(FooterItemEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<FooterItemEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<FooterItemEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<FooterItemEntity> UpdateAsync(FooterItemEntity entity, CancellationToken cancellationToken);
    }
}
