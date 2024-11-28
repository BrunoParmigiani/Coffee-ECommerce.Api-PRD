namespace Coffee_Ecommerce.API.Features.ApiAccess.Repository
{
    public interface IApiAccessRepository
    {
        public Task<ApiAccessEntity> CreateAsync(ApiAccessEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<ApiAccessEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<ApiAccessEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<bool> RemoveKeyAsync(Guid id, CancellationToken cancellationToken);
        public Task<ApiAccessEntity> RefreshKeyAsync(Guid entityId, string key, CancellationToken cancellationToken);
        public Task<bool> ValidateKeyAsync(string key, CancellationToken cancellationToken);
    }
}
