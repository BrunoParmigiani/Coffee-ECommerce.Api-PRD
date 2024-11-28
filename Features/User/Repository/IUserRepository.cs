using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Coffee_Ecommerce.API.Features.User.PasswordChange;

namespace Coffee_Ecommerce.API.Features.User.Repository
{
    public interface IUserRepository
    {
        public Task<UserEntity> CreateAsync(UserEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<UserEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UserEntity> UpdateAsync(UserEntity entity, CancellationToken cancellationToken);
        public Task<bool> RecoverAccountAsync(Guid id, CancellationToken cancellationToken);

        public Task<UserEntity> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken);
        public Task<UserEntity> ValidateCredentialsAsync(string email, CancellationToken cancellationToken);
        public Task<bool> RevokeTokenAsync(string email, CancellationToken cancellationToken);
        public Task<UserEntity> RefreshInfoAsync(UserEntity entity, CancellationToken cancellationToken);

        public Task<bool> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken);
    }
}
