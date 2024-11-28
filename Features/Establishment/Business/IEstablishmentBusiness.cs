using Coffee_Ecommerce.API.Features.Establishment.Block;
using Coffee_Ecommerce.API.Features.Establishment.Create;
using Coffee_Ecommerce.API.Features.Establishment.Delete;
using Coffee_Ecommerce.API.Features.Establishment.GetAll;
using Coffee_Ecommerce.API.Features.Establishment.GetById;
using Coffee_Ecommerce.API.Features.Establishment.PasswordChange;
using Coffee_Ecommerce.API.Features.Establishment.Update;

namespace Coffee_Ecommerce.API.Features.Establishment.Business
{
    public interface IEstablishmentBusiness
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
