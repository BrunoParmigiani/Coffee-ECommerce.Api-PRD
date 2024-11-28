using Coffee_Ecommerce.API.Features.Report.Create;
using Coffee_Ecommerce.API.Features.Report.Delete;
using Coffee_Ecommerce.API.Features.Report.GetAll;
using Coffee_Ecommerce.API.Features.Report.GetById;
using Coffee_Ecommerce.API.Features.Report.GetPage;
using Coffee_Ecommerce.API.Features.Report.Update;

namespace Coffee_Ecommerce.API.Features.Report.Business
{
    public interface IReportBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetPageResult> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
    }
}
