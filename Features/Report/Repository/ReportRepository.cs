using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Order.Exceptions;
using Coffee_Ecommerce.API.Features.Report.Enums;
using Coffee_Ecommerce.API.Features.Report.Exceptions;
using Coffee_Ecommerce.API.Features.Report.GetPage;
using Coffee_Ecommerce.API.Features.User.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Coffee_Ecommerce.API.Features.Report.Repository
{
    public sealed class ReportRepository : IReportRepository
    {
        private readonly PostgreContext _context;

        public ReportRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<ReportEntity> CreateAsync(ReportEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var reportExists = await _context.Reports.AnyAsync(report => report.OrderId == entity.OrderId);

            if (reportExists)
                throw new ArgumentException("A report has already been opened for this order");

            var orderExists = await _context.Orders.AnyAsync(order => order.Id == entity.OrderId);

            if (!orderExists)
                throw new OrderNotFoundException("Order not found");

            var userExists = await _context.Users.AnyAsync(user => user.Id == entity.UserId);

            if (!userExists)
                throw new UserNotFoundException("Invalid user");

            var establishmentExists = await _context.Establishments.AnyAsync(establishment => establishment.Id == entity.EstablishmentId);

            if (!establishmentExists)
                throw new EstablishmentNotFoundException("Establishment not found");

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                _context.Remove(result);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (ReportNotFoundException ex)
            {
                throw new ReportNotFoundException(ex.Message);
            }
        }

        public async Task<List<ReportEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Reports.ToListAsync(cancellationToken);
        }

        public async Task<ReportEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Reports.SingleOrDefaultAsync(report => report.Id == id);

            if (result == null)
                throw new ReportNotFoundException("Report not found");

            return result;
        }

        public async Task<List<ReportEntity>> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken)
        {
            int position = command.Items * (command.Page - 1);

            var filteredReports = await GetFilteredAsync(command, cancellationToken);

            List<ReportEntity> orderedReports = new List<ReportEntity>();

            switch (command.OrderBy)
            {
                case "date_ascending":
                    orderedReports = filteredReports.OrderBy(report => report.OpenTime).ToList();
                    break;
                case "date_descending":
                    orderedReports = filteredReports.OrderByDescending(report => report.OpenTime).ToList();
                    break;
                default:
                    orderedReports = filteredReports;
                    break;
            }

            var result = orderedReports
                .Skip(position)
                .Take(command.Items)
                .ToList();

            if (result.IsNullOrEmpty())
                throw new ReportNotFoundException("There are no associated reports to this page");

            return result;
        }

        private async Task<List<ReportEntity>> GetFilteredAsync(GetPageCommand command, CancellationToken cancellationToken)
        {
            List<ReportEntity> reports = await _context.Reports.ToListAsync(cancellationToken);

            if (command.EstablishmentId != Guid.Empty && command.EstablishmentId != null)
                reports = reports.Where(report => report.EstablishmentId == command.EstablishmentId).ToList();

            if (command.CustomerId != Guid.Empty && command.CustomerId != null)
                reports = reports.Where(report => report.UserId == command.CustomerId).ToList();

            if (command.Date != DateTime.MinValue && command.Date != null)
                reports = reports.Where(report => report.OpenTime.Date == command.Date.Value.Date).ToList();

            if (command.Status >= 0)
                reports = reports.Where(report => report.Status == command.Status).ToList();

            return reports;
        }

        public async Task<ReportEntity> UpdateAsync(ReportEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be empty");

            var report = await _context.Reports
                .AsNoTracking()
                .SingleOrDefaultAsync(report => report.Id == entity.Id, cancellationToken);

            if (report == null)
                throw new ReportNotFoundException("Report not found");

            if (report.Status == (int)Statuses.Closed)
                throw new ArgumentException("Cannot update closed report");

            // User do not change
            // Order do not change
            // Establishment do not change
            // OpenTime do not change
            // Problem do not change
            // Description do not change
            // I should kill myself for that
            entity.UserId = report.UserId;
            entity.OrderId = report.OrderId;
            entity.EstablishmentId = report.EstablishmentId;
            entity.OpenTime = report.OpenTime.ToUniversalTime();
            entity.Problem = report.Problem;
            entity.Description = report.Description;

            /*if (!report.UserId.Equals(entity.UserId)
                || !report.OpenTime.Equals(entity.OpenTime)
                || !report.Problem.Equals(entity.Problem)
                || !report.Description.Equals(entity.Description)
            )
                throw new ArgumentException("Forbidden property change");*/

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
