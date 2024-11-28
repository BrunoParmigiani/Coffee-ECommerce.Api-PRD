using Coffee_Ecommerce.API.Features.Announcement.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Features.Announcement.Repository
{
    public sealed class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly PostgreContext _context;

        public AnnouncementRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<AnnouncementEntity> CreateAsync(AnnouncementEntity entity, CancellationToken cancellationToken)
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
            catch (AnnouncementNotFoundException ex)
            {
                throw new AnnouncementNotFoundException(ex.Message);
            }
        }

        public async Task<List<AnnouncementEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Announcements.ToListAsync(cancellationToken);
        }

        public async Task<AnnouncementEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Announcements.SingleOrDefaultAsync(announcement => announcement.Id == id);

            if (result == null)
                throw new AnnouncementNotFoundException("Announcement not found");

            return result;
        }

        public async Task<List<AnnouncementEntity>> GetByRoleAsync(int roleNumber, Guid userId, CancellationToken cancellationToken)
        {
            var result = _context.Announcements
                .Where(announcement => announcement.Recipients.Contains(roleNumber.ToString())
                || announcement.CreatorId == userId);

            return await result.ToListAsync();
        }

        public async Task<AnnouncementEntity> UpdateAsync(AnnouncementEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var announcement = await _context.Announcements
                .AsNoTracking()
                .SingleOrDefaultAsync(announcement => announcement.Id == entity.Id, cancellationToken);

            if (announcement == null)
                throw new AnnouncementNotFoundException("Announcement not found");

            if (entity.CreatorId != announcement.CreatorId)
                throw new ArgumentException("Forbidden operation");

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
