using Coffee_Ecommerce.API.Features.ApiAccess.Create;
using Coffee_Ecommerce.API.Features.ApiAccess.Delete;
using Coffee_Ecommerce.API.Features.ApiAccess.GetAll;
using Coffee_Ecommerce.API.Features.ApiAccess.GetById;
using Coffee_Ecommerce.API.Features.ApiAccess.RefreshKey;
using Coffee_Ecommerce.API.Features.ApiAccess.RemoveKey;
using Coffee_Ecommerce.API.Features.ApiAccess.Validate;

namespace Coffee_Ecommerce.API.Features.ApiAccess.Business
{
    public interface IApiAccessBusiness
    {
        public Task<CreateResult> CreateAsync(string serviceName, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<RemoveKeyResult> RemoveKeyAsync(Guid id, CancellationToken cancellationToken);
        public Task<RefreshKeyResult> RefreshKeyAsync(Guid entityId, string key, CancellationToken cancellationToken);
        public Task<ValidateKeyResult> ValidateKeyAsync(string key, CancellationToken cancellationToken);
    }
}
