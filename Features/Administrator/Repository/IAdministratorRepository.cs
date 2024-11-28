using Coffee_Ecommerce.API.Features.Administrator.PasswordChange;
using Coffee_Ecommerce.API.Features.Authentication.Signin;

namespace Coffee_Ecommerce.API.Features.Administrator.Repository
{
    public interface IAdministratorRepository
    {
        public Task<AdministratorEntity> CreateAsync(AdministratorEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<AdministratorEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<AdministratorEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<AdministratorEntity> UpdateAsync(AdministratorEntity entity, CancellationToken cancellationToken);
        public Task<Tuple<bool, string?>> BlockAsync(Guid id, CancellationToken cancellationToken);

        public Task<AdministratorEntity> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken);
        public Task<AdministratorEntity> ValidateCredentialsAsync(string email, CancellationToken cancellationToken);
        public Task<bool> RevokeTokenAsync(string email, CancellationToken cancellationToken);
        public Task<AdministratorEntity> RefreshInfoAsync(AdministratorEntity entity, CancellationToken cancellationToken);

        public Task<bool> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken);
    }
}
