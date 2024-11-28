using Coffee_Ecommerce.API.Infraestructure;
using Coffee_Ecommerce.API.Features.Administrator.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Coffee_Ecommerce.API.Features.Administrator.PasswordChange;
using Coffee_Ecommerce.API.Identity;

namespace Coffee_Ecommerce.API.Features.Administrator.Repository
{
    public sealed class AdministratorRepository : IAdministratorRepository
    {
        private readonly PostgreContext _context;

        public AdministratorRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<AdministratorEntity> CreateAsync(AdministratorEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var result = await _context.Administrators.AnyAsync(admin => admin.Email == entity.Email || admin.CPF == entity.CPF, cancellationToken);

            if (result)
                throw new ValuesAlreadyInUseException("Email or CPF already in use");

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

                if (result.Role == Roles.Owner)
                    throw new InvalidOperationException("Cannot delete an owner account");

                if (!result.Blocked)
                    throw new InvalidOperationException("The account must be blocked before being scheduled for deletion");

                result.DeleteSchedule = DateTime.UtcNow.AddDays(30);

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (AdministratorNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<AdministratorEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Administrators.ToListAsync(cancellationToken);
        }

        public async Task<AdministratorEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Administrators.SingleOrDefaultAsync(admin => admin.Id == id, cancellationToken);

            if (result == null)
                throw new AdministratorNotFoundException("Administrator not found");

            return result;
        }

        public async Task<AdministratorEntity> UpdateAsync(AdministratorEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var admin = await _context.Administrators
                .AsNoTracking()
                .SingleOrDefaultAsync(admin => admin.Id == entity.Id, cancellationToken);

            if (admin == null)
                throw new AdministratorNotFoundException("Administrator not found");

            var valuesInUse = await _context.Administrators.AnyAsync(admin => admin.Id != entity.Id
                && (admin.Email == entity.Email || admin.CPF == entity.CPF), cancellationToken);

            if (valuesInUse)
                throw new ValuesAlreadyInUseException("Email or CPF already in use");

            entity.Password = admin.Password;
            entity.Role = admin.Role;

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<Tuple<bool, string?>> BlockAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                if (result.Role == Roles.Owner)
                    throw new InvalidOperationException("Cannot block an owner account");

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
                    result.Role = Roles.Administrator;
                    result.DeleteSchedule = DateTime.MaxValue.ToUniversalTime();

                    await _context.SaveChangesAsync(cancellationToken);

                    return new Tuple<bool, string?>(true, "Account unblocked");
                }

                return new Tuple<bool, string?>(false, null);
            }
            catch (AdministratorNotFoundException)
            {
                throw;
            }
        }

        public async Task<AdministratorEntity> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            var encryptedPassword = ComputeHash(credentials.Password, SHA256.Create());

            var result = await _context.Administrators.SingleOrDefaultAsync(admin => admin.Email == credentials.Email
                && admin.Password == encryptedPassword, cancellationToken);

            return result;
        }

        public async Task<AdministratorEntity> ValidateCredentialsAsync(string email, CancellationToken cancellationToken)
        {
            var result = await _context.Administrators.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

            return result;
        }

        public async Task<bool> RevokeTokenAsync(string email, CancellationToken cancellationToken)
        {
            var admin = await _context.Administrators.SingleOrDefaultAsync(admin => admin.Email == email, cancellationToken);

            if (admin == null)
                return false;

            admin.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }

        public async Task<AdministratorEntity> RefreshInfoAsync(AdministratorEntity entity, CancellationToken cancellationToken)
        {
            var result = await _context.Administrators.SingleOrDefaultAsync(admin => admin.Id == entity.Id, cancellationToken);

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

            var admin = await _context.Administrators.SingleOrDefaultAsync(admin => admin.Email == credentials.Email
                && admin.Password == encryptedCurrentPassword, cancellationToken);

            if (admin == null)
                throw new AdministratorNotFoundException("Administrator not found");

            admin.Password = encryptedNewPassword;

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
