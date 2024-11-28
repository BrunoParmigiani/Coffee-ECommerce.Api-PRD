namespace Coffee_Ecommerce.API.Features.Announcement.Repository
{
    public interface IAnnouncementRepository
    {
        public Task<AnnouncementEntity> CreateAsync(AnnouncementEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<AnnouncementEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<AnnouncementEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<AnnouncementEntity>> GetByRoleAsync(int roleNumber, Guid userId, CancellationToken cancellationToken);
        public Task<AnnouncementEntity> UpdateAsync(AnnouncementEntity entity, CancellationToken cancellationToken);
    }
}
