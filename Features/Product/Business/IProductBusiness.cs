using Coffee_Ecommerce.API.Features.Product.Create;
using Coffee_Ecommerce.API.Features.Product.Delete;
using Coffee_Ecommerce.API.Features.Product.GetAll;
using Coffee_Ecommerce.API.Features.Product.GetByEstablishment;
using Coffee_Ecommerce.API.Features.Product.GetById;
using Coffee_Ecommerce.API.Features.Product.Update;

namespace Coffee_Ecommerce.API.Features.Product.Business
{
    public interface IProductBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByEstablishmentResult> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
    }
}
