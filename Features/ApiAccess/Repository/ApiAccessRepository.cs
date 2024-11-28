using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Coffee_Ecommerce.API.Features.ApiAccess.Repository
{
    public sealed class ApiAccessRepository : IApiAccessRepository
    {
        private readonly PostgreContext _context;

        public ApiAccessRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<ApiAccessEntity> CreateAsync(ApiAccessEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)} cannot be null");

            var nameInUse = await _context.ApiAccesses.AnyAsync(access => access.ServiceName == entity.ServiceName);

            if (nameInUse)
                throw new ArgumentException($"\"{entity.ServiceName}\" name already in use");

            entity.Key = ComputeHash(entity.Key, SHA256.Create());

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var access = await GetByIdAsync(id, cancellationToken);

                _context.Remove(access);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<ApiAccessEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ApiAccesses.ToListAsync(cancellationToken);
        }

        public async Task<ApiAccessEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var access = await _context.ApiAccesses.SingleOrDefaultAsync(access => access.Id == id);

            if (access == null)
                throw new KeyNotFoundException("Registry not found");

            return access;
        }

        public async Task<bool> RemoveKeyAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                result.Key = null;

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        public async Task<ApiAccessEntity> RefreshKeyAsync(Guid entityId, string key, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(entityId, cancellationToken);

                result.Key = ComputeHash(key, SHA256.Create());

                return result;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private string ComputeHash(string key, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(key);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var item in hashedBytes)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }

        public async Task<bool> ValidateKeyAsync(string key, CancellationToken cancellationToken)
        {
            string encryptedKey = ComputeHash(key, SHA256.Create());

            var result = await _context.ApiAccesses.AnyAsync(a => a.Key == encryptedKey, cancellationToken);

            return result;
        }
    }
}
