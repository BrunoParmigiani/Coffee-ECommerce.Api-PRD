using Coffee_Ecommerce.API.Features.FooterItem.Create;
using Coffee_Ecommerce.API.Features.FooterItem.Delete;
using Coffee_Ecommerce.API.Features.FooterItem.GetAll;
using Coffee_Ecommerce.API.Features.FooterItem.GetById;
using Coffee_Ecommerce.API.Features.FooterItem.Update;

namespace Coffee_Ecommerce.API.Features.FooterItem.Business
{
    public interface IFooterItemBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
    }
}
