using Coffee_Ecommerce.API.Features.Administrator.Exceptions;
using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Establishment.PasswordChange;
using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using ValuesAlreadyInUseException = Coffee_Ecommerce.API.Features.Establishment.Exceptions.ValuesAlreadyInUseException;

namespace Coffee_Ecommerce.API.Features.Establishment.Repository
{
    public sealed class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly PostgreContext _context;

        public EstablishmentRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<EstablishmentEntity> CreateAsync(EstablishmentEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var result = await _context.Establishments.AnyAsync(establishment => establishment.Email == entity.Email, cancellationToken);

            if (result)
                throw new ValuesAlreadyInUseException("Email already in use");

            var adminExists = await _context.Administrators.AnyAsync(admin => admin.Id == entity.AdministratorId);

            if (!adminExists)
                throw new AdministratorNotFoundException("Invalid administrator");

            entity.Password = ComputeHash(entity.Password, SHA256.Create());

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                if (!result.Blocked)
                    throw new InvalidOperationException("The account must be blocked before being scheduled for deletion");

                result.DeleteSchedule = DateTime.UtcNow.AddDays(30);

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (EstablishmentNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<EstablishmentEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Establishments.ToListAsync(cancellationToken);
        }

        public async Task<EstablishmentEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Establishments.SingleOrDefaultAsync(establishment => establishment.Id == id);

            if (result == null)
                throw new EstablishmentNotFoundException("Establishment not found");

            return result;
        }

        public async Task<EstablishmentEntity> UpdateAsync(EstablishmentEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var establishment = await _context.Establishments
                .AsNoTracking()
                .SingleOrDefaultAsync(establishment => establishment.Id == entity.Id, cancellationToken);

            if (establishment == null)
                throw new EstablishmentNotFoundException("Establishment not found");

            var valuesInUse = await _context.Establishments.AnyAsync(establishment => establishment.Id != entity.Id
                && establishment.Email == entity.Email, cancellationToken);

            if (valuesInUse)
                throw new ValuesAlreadyInUseException("Email already in use");

            var adminExists = await _context.Administrators.AnyAsync(admin => admin.Id == entity.AdministratorId);

            if (!adminExists)
                throw new AdministratorNotFoundException("Invalid administrator");

            entity.Password = establishment.Password;

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<Tuple<bool, string?>> BlockAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                if (!result.Blocked)
                {
                    result.Blocked = true;
                    result.Role = Roles.BlockedAccount;

                    await _context.SaveChangesAsync(cancellationToken);

                    return new Tuple<bool, string?>(true, "Account blocked");
                }
                else if (result.Blocked)
                {
                    result.Blocked = false;
                    result.Role = Roles.Establishment;
                    result.DeleteSchedule = DateTime.MaxValue.ToUniversalTime();

                    await _context.SaveChangesAsync(cancellationToken);

                    return new Tuple<bool, string?>(true, "Account unblocked");
                }

                return new Tuple<bool, string?>(true, null);
            }
            catch (EstablishmentNotFoundException)
            {
                throw;
            }
        }

        public async Task<EstablishmentEntity> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            var encryptedPassword = ComputeHash(credentials.Password, SHA256.Create());

            var result = await _context.Establishments.SingleOrDefaultAsync(establishment => establishment.Email == credentials.Email && establishment.Password == encryptedPassword, cancellationToken);

            return result;
        }

        public async Task<EstablishmentEntity> ValidateCredentialsAsync(string email, CancellationToken cancellationToken)
        {
            var result = await _context.Establishments.SingleOrDefaultAsync(establishment => establishment.Email == email, cancellationToken);

            return result;
        }

        public async Task<bool> RevokeTokenAsync(string email, CancellationToken cancellationToken)
        {
            var establishment = await _context.Establishments.SingleOrDefaultAsync(establishment => establishment.Email == email, cancellationToken);

            if (establishment == null)
                return false;

            establishment.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }

        public async Task<EstablishmentEntity> RefreshInfoAsync(EstablishmentEntity entity, CancellationToken cancellationToken)
        {
            var result = await _context.Establishments.SingleOrDefaultAsync(establishment => establishment.Id == entity.Id, cancellationToken);

            try
            {
                _context.Entry(result).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        public async Task<bool> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken)
        {
            string encryptedCurrentPassword = ComputeHash(credentials.CurrentPassword, SHA256.Create());
            string encryptedNewPassword = ComputeHash(credentials.NewPassword, SHA256.Create());

            var establishment = await _context.Establishments.SingleOrDefaultAsync(establishment => establishment.Email == credentials.Email
                && establishment.Password == encryptedCurrentPassword, cancellationToken);

            if (establishment == null)
                throw new EstablishmentNotFoundException("Establishment not found");

            establishment.Password = encryptedNewPassword;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        private string ComputeHash(string password, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var item in hashedBytes)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }

    }
}
