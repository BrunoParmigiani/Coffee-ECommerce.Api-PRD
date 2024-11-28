using Coffee_Ecommerce.API.Features.Resume.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Features.Resume.Repository
{
    public sealed class ResumeRepository : IResumeRepository
    {
        private readonly PostgreContext _context;

        public ResumeRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<ResumeEntity> CreateAsync(ResumeEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var resumeAlreadyRegistered = await _context.Resumes.AnyAsync(resume => resume.UserId == entity.UserId);

            if (resumeAlreadyRegistered)
                throw new ArgumentException("A resume is already registered for this user");

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
            catch (ResumeNotFoundException ex)
            {
                throw new ResumeNotFoundException(ex.Message);
            }
        }

        public async Task<List<ResumeEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Resumes.ToListAsync(cancellationToken);
        }

        public async Task<ResumeEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Resumes.SingleOrDefaultAsync(report => report.Id == id);

            if (result == null)
                throw new ResumeNotFoundException("Resume not found");

            return result;
        }

        public async Task<ResumeEntity> UpdateAsync(ResumeEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var exists = await _context.Resumes.AnyAsync(report => report.Id == entity.Id
                && report.UserId == entity.UserId, cancellationToken);

            if (!exists)
                throw new ResumeNotFoundException("Resume not found");

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
