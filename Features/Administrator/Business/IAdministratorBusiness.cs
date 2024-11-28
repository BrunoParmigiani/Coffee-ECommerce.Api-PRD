using Coffee_Ecommerce.API.Features.Administrator.Block;
using Coffee_Ecommerce.API.Features.Administrator.Create;
using Coffee_Ecommerce.API.Features.Administrator.Delete;
using Coffee_Ecommerce.API.Features.Administrator.GetAll;
using Coffee_Ecommerce.API.Features.Administrator.GetById;
using Coffee_Ecommerce.API.Features.Administrator.PasswordChange;
using Coffee_Ecommerce.API.Features.Administrator.Update;

namespace Coffee_Ecommerce.API.Features.Administrator.Business
{
    public interface IAdministratorBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
        public Task<PasswordChangeResult> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken);
        public Task<BlockResult> BlockAsync(Guid id, CancellationToken cancellationToken);
    }
}
