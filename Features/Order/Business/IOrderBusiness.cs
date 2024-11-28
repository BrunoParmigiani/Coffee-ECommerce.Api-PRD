using Coffee_Ecommerce.API.Features.Order.Create;
using Coffee_Ecommerce.API.Features.Order.Delete;
using Coffee_Ecommerce.API.Features.Order.GetAll;
using Coffee_Ecommerce.API.Features.Order.GetById;
using Coffee_Ecommerce.API.Features.Order.GetPage;
using Coffee_Ecommerce.API.Features.Order.Rate;
using Coffee_Ecommerce.API.Features.Order.Update;

namespace Coffee_Ecommerce.API.Features.Order.Business
{
    public interface IOrderBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetPageResult> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
        public Task<RateResult> RateAsync(RateCommand command, CancellationToken cancellationToken);
    }
}
