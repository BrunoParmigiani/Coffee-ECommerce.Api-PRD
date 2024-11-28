using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Order.Create;
using Coffee_Ecommerce.API.Features.Order.Delete;
using Coffee_Ecommerce.API.Features.Order.DTO;
using Coffee_Ecommerce.API.Features.Order.Exceptions;
using Coffee_Ecommerce.API.Features.Order.GetAll;
using Coffee_Ecommerce.API.Features.Order.GetById;
using Coffee_Ecommerce.API.Features.Order.GetPage;
using Coffee_Ecommerce.API.Features.Order.Rate;
using Coffee_Ecommerce.API.Features.Order.Repository;
using Coffee_Ecommerce.API.Features.Order.Update;
using Coffee_Ecommerce.API.Features.User.Exceptions;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Order.Business
{
    public sealed class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository _repository;
        private readonly OrderParser _parser;

        public OrderBusiness(IOrderRepository repository)
        {
            _repository = repository;
            _parser = new OrderParser();
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            OrderEntity entity;

            if (!command.DeniedOrder)
            {
                entity = new OrderEntity
                {
                    UserId = sanitizedCommand.UserId,
                    TotalValue = sanitizedCommand.GetTotalValue(),
                    PaymentMethod = sanitizedCommand.PaymentMethod,
                    Paid = false,
                    EstablishmentId = sanitizedCommand.EstablishmentId,
                    DeliveryTime = new TimeSpan(0, 0, sanitizedCommand.DeliveryTime),
                    DeliveredAtTime = DateTime.MinValue,
                    OrderedAt = DateTime.UtcNow,
                    Items = sanitizedCommand.GetSerializedItems()
                };
            }
            else
            {
                entity = new OrderEntity
                {
                    UserId = sanitizedCommand.UserId,
                    TotalValue = sanitizedCommand.GetTotalValue(),
                    PaymentMethod = sanitizedCommand.PaymentMethod,
                    Paid = false,
                    EstablishmentId = sanitizedCommand.EstablishmentId,
                    DeliveryTime = new TimeSpan(0, 0, 0),
                    DeliveredAtTime = DateTime.MinValue,
                    OrderedAt = DateTime.UtcNow,
                    DeniedOrder = sanitizedCommand.DeniedOrder,
                    DeniedReason = sanitizedCommand.DeniedReason,
                    Items = sanitizedCommand.GetSerializedItems()
                };
            }

            try
            {
                var result = await _repository.CreateAsync(entity, cancellationToken);

                return new CreateResult { Data = _parser.Parse(result) };
            }
            catch (ArgumentNullException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (UserNotFoundException ex)
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
            catch (OrderNotFoundException ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);

            return new GetAllResult { Data = _parser.Parse(result) };
        }

        public async Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = GetByIdValidator.CheckForErrors(id);

            if (validationResult != null)
                return new GetByIdResult { Error = validationResult };

            try
            {
                var result = await _repository.GetByIdAsync(id, cancellationToken);

                return new GetByIdResult { Data = _parser.Parse(result) };
            }
            catch (OrderNotFoundException ex)
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

                return new GetPageResult { Data = _parser.Parse(result) };
            }
            catch (OrderNotFoundException ex)
            {
                return new GetPageResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken)
        {
            var validationResult = UpdateValidator.CheckForErrors(command);

            if (validationResult != null)
                return new UpdateResult { Error = validationResult };

            var entity = new OrderEntity
            {
                Id = command.Id,
                Paid = command.Paid,
                Delivered = command.Delivered,
                DeliveredAtTime = command.Delivered ? DateTime.UtcNow : DateTime.MinValue,
                DeliveryTime = new TimeSpan(0, 0, command.DeliveryTime)
            };

            try
            {
                var result = await _repository.UpdateAsync(entity, cancellationToken);

                return new UpdateResult { Data = _parser.Parse(result) };
            }
            catch (ArgumentNullException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (OrderNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ArgumentException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (InvalidOperationException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (Exception ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<RateResult> RateAsync(RateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = RateValidator.Sanitize(command);
            var validationResult = RateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new RateResult { Error = validationResult };

            var entity = new OrderEntity
            {
                Id = sanitizedCommand.Id,
                Rated = true,
                TimeRating = sanitizedCommand.TimeRating,
                QualityRating = sanitizedCommand.QualityRating,
                UserComments = sanitizedCommand.UserComments
            };

            try
            {
                var result = await _repository.RateAsync(entity, cancellationToken);

                return new RateResult { Data = _parser.Parse(result) };
            }
            catch (Exception ex)
            {
                return new RateResult { Error = new ApiError(ex.Message) };
            }
        }
    }
}
