using Coffee_Ecommerce.API.Features.User.Create;
using Coffee_Ecommerce.API.Features.User.Delete;
using Coffee_Ecommerce.API.Features.User.GetAll;
using Coffee_Ecommerce.API.Features.User.GetById;
using Coffee_Ecommerce.API.Features.User.PasswordChange;
using Coffee_Ecommerce.API.Features.User.Recovery;
using Coffee_Ecommerce.API.Features.User.Update;

namespace Coffee_Ecommerce.API.Features.User.Business
{
    public interface IUserBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
        public Task<RecoveryResult> RecoverAccountAsync(Guid id, CancellationToken cancellationToken);
        public Task<PasswordChangeResult> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken);
    }
}
