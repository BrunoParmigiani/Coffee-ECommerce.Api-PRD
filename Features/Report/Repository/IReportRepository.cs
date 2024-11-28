using Coffee_Ecommerce.API.Features.Report.GetPage;

namespace Coffee_Ecommerce.API.Features.Report.Repository
{
    public interface IReportRepository
    {
        public Task<ReportEntity> CreateAsync(ReportEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<ReportEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<ReportEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<ReportEntity>> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken);
        public Task<ReportEntity> UpdateAsync(ReportEntity entity, CancellationToken cancellationToken);
    }
}
