namespace Coffee_Ecommerce.API.Features.Resume.Repository
{
    public interface IResumeRepository
    {
        public Task<ResumeEntity> CreateAsync(ResumeEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<ResumeEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<ResumeEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<ResumeEntity> UpdateAsync(ResumeEntity entity, CancellationToken cancellationToken);
    }
}
