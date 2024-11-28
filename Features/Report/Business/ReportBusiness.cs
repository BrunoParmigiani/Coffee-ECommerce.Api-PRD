using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Order.Exceptions;
using Coffee_Ecommerce.API.Features.Report.Create;
using Coffee_Ecommerce.API.Features.Report.Delete;
using Coffee_Ecommerce.API.Features.Report.Enums;
using Coffee_Ecommerce.API.Features.Report.Exceptions;
using Coffee_Ecommerce.API.Features.Report.GetAll;
using Coffee_Ecommerce.API.Features.Report.GetById;
using Coffee_Ecommerce.API.Features.Report.GetPage;
using Coffee_Ecommerce.API.Features.Report.Repository;
using Coffee_Ecommerce.API.Features.Report.Update;
using Coffee_Ecommerce.API.Features.User.Exceptions;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Report.Business
{
    public sealed class ReportBusiness : IReportBusiness
    {
        private readonly IReportRepository _repository;

        public ReportBusiness(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            var entity = new ReportEntity
            {
                UserId = sanitizedCommand.UserId,
                EstablishmentId = sanitizedCommand.EstablishmentId,
                OrderId = sanitizedCommand.OrderId,
                OpenTime = DateTime.Now.ToUniversalTime(),
                CloseTime = DateTime.MinValue,
                Problem = sanitizedCommand.Problem,
                Description = sanitizedCommand.Description,
                Status = 0
            };

            try
            {
                var result = await _repository.CreateAsync(entity, cancellationToken);

                return new CreateResult { Data = result };
            }
            catch (ArgumentNullException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (UserNotFoundException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (ArgumentException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (OrderNotFoundException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (EstablishmentNotFoundException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = DeleteValidator.CheckForErrors(id);

            if (validationResult != null)
                return new DeleteResult { Error = validationResult };

            try
            {
                var result = await _repository.DeleteAsync(id, cancellationToken);

                return new DeleteResult { Data = result };
            }
            catch (ReportNotFoundException ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);

            return new GetAllResult { Data = result };
        }

        public async Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = GetByIdValidator.CheckForErrors(id);

            if (validationResult != null)
                return new GetByIdResult { Error = validationResult };

            try
            {
                var result = await _repository.GetByIdAsync(id, cancellationToken);

                return new GetByIdResult { Data = result };
            }
            catch (ReportNotFoundException ex)
            {
                return new GetByIdResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetPageResult> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken)
        {
            var validationResult = GetPageValidator.CheckForErrors(command);

            if (validationResult != null)
                return new GetPageResult { Error = validationResult };

            try
            {
                var result = await _repository.GetPageAsync(command, cancellationToken);

                return new GetPageResult { Data = result };
            }
            catch (ReportNotFoundException ex)
            {
                return new GetPageResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = UpdateValidator.Sanitize(command);
            var validationResult = UpdateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new UpdateResult { Error = validationResult };

            var entity = new ReportEntity
            {
                Id = command.Id,
                CloseTime = sanitizedCommand.Status == (int)Statuses.Closed ? DateTime.UtcNow : DateTime.MinValue,
                Status = sanitizedCommand.Status
            };

            try
            {
                var result = await _repository.UpdateAsync(entity, cancellationToken);

                return new UpdateResult { Data = result };
            }
            catch (ArgumentNullException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ReportNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ArgumentException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
        }
    }
}
