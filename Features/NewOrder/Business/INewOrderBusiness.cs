using Coffee_Ecommerce.API.Features.NewOrder.Create;
using Coffee_Ecommerce.API.Features.NewOrder.Delete;
using Coffee_Ecommerce.API.Features.NewOrder.GetAll;

namespace Coffee_Ecommerce.API.Features.NewOrder.Business
{
    public interface INewOrderBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken);
    }
}
