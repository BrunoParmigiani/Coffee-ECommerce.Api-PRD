using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Coffee_Ecommerce.API.Features.User.Exceptions;
using Coffee_Ecommerce.API.Features.User.PasswordChange;
using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Coffee_Ecommerce.API.Features.User.Repository
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly PostgreContext _context;

        public UserRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> CreateAsync(UserEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var result = await _context.Users.AnyAsync(user => user.Email == entity.Email || user.CPF == entity.CPF, cancellationToken);

            if (result)
                throw new ValuesAlreadyInUseException("Values already in use");

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

                if (!result.Suspended)
                {
                    result.Suspended = true;
                    result.DeleteSchedule = DateTime.UtcNow.AddDays(30);
                    result.Role = Roles.SuspendedUser;

                    await _context.SaveChangesAsync(cancellationToken);

                    return true;
                }

                throw new InvalidOperationException("Account already schedule for deletion");
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }

        public async Task<UserEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (result == null)
                throw new UserNotFoundException("User not found");

            return result;
        }

        public async Task<UserEntity> UpdateAsync(UserEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Id == entity.Id, cancellationToken);

            if (user == null)
                throw new UserNotFoundException("User not found");

            var valuesInUse = await _context.Users.AnyAsync(user => user.Id != entity.Id
                && (user.Email == entity.Email || user.CPF == entity.CPF), cancellationToken);

            if (valuesInUse)
                throw new ValuesAlreadyInUseException("Email already in use");

            entity.Password = user.Password;
            entity.Role = user.Role;

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> RecoverAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                if (!result.Suspended)
                    throw new InvalidOperationException("This account is not suspended");

                result.Suspended = false;
                result.DeleteSchedule = DateTime.MaxValue.ToUniversalTime();
                result.Role = Roles.User;

                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }

        public async Task<UserEntity> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            var encryptedPassword = ComputeHash(credentials.Password, SHA256.Create());

            var result = await _context.Users.SingleOrDefaultAsync(user => user.Email == credentials.Email && user.Password == encryptedPassword, cancellationToken);

            return result;
        }

        public async Task<UserEntity> ValidateCredentialsAsync(string email, CancellationToken cancellationToken)
        {
            var result = await _context.Users.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

            return result;
        }

        public async Task<bool> RevokeTokenAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

            if (user == null)
                return false;

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }

        public async Task<UserEntity> RefreshInfoAsync(UserEntity entity, CancellationToken cancellationToken)
        {
            var result = await _context.Users.SingleOrDefaultAsync(user => user.Id == entity.Id, cancellationToken);

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

            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == credentials.Email
                && user.Password == encryptedCurrentPassword, cancellationToken);

            if (user == null)
                throw new UserNotFoundException("User not found");

            user.Password = encryptedNewPassword;

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
