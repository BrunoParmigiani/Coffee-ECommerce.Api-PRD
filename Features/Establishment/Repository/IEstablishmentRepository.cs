using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Coffee_Ecommerce.API.Features.Establishment.PasswordChange;

namespace Coffee_Ecommerce.API.Features.Establishment.Repository
{
    public interface IEstablishmentRepository
    {
        public Task<EstablishmentEntity> CreateAsync(EstablishmentEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<EstablishmentEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<EstablishmentEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<EstablishmentEntity> UpdateAsync(EstablishmentEntity entity, CancellationToken cancellationToken);
        public Task<Tuple<bool, string?>> BlockAsync(Guid id, CancellationToken cancellationToken);

        public Task<EstablishmentEntity> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken);
        public Task<EstablishmentEntity> ValidateCredentialsAsync(string email, CancellationToken cancellationToken);
        public Task<bool> RevokeTokenAsync(string email, CancellationToken cancellationToken);
        public Task<EstablishmentEntity> RefreshInfoAsync(EstablishmentEntity entity, CancellationToken cancellationToken);

        public Task<bool> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken);
    }
}
