using Coffee_Ecommerce.API.Features.NewOrder.Create;
using Coffee_Ecommerce.API.Features.NewOrder.Delete;
using Coffee_Ecommerce.API.Features.NewOrder.DTO;
using Coffee_Ecommerce.API.Features.NewOrder.Exceptions;
using Coffee_Ecommerce.API.Features.NewOrder.GetAll;
using Coffee_Ecommerce.API.Features.NewOrder.Repository;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.NewOrder.Business
{
    public sealed class NewOrderBusiness : INewOrderBusiness
    {
        private readonly INewOrderRepository _repository;
        private readonly NewOrderParser _parser;

        public NewOrderBusiness(INewOrderRepository repository)
        {
            _repository = repository;
            _parser = new NewOrderParser();
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult is not null)
                return new CreateResult { Error = validationResult };

            NewOrderEntity newOrder = new NewOrderEntity
            {
                Id = sanitizedCommand.Id,
                UserId = sanitizedCommand.UserId,
                PaymentMethod = sanitizedCommand.PaymentMethod,
                EstablishmentId = sanitizedCommand.EstablishmentId,
                DeliveryTime = new TimeSpan(0, 0, 0),
                Items = sanitizedCommand.GetSerializedItems(),
                DeniedOrder = false,
                UserName = sanitizedCommand.UserName,
                UserAddress = sanitizedCommand.UserAddress,
                UserComplement = sanitizedCommand.UserComplement,
                DeniedReason = null
            };

            try
            {
                var result = await _repository.CreateAsync(newOrder, cancellationToken);

                return new CreateResult { Data = result };
            }
            catch (ArgumentNullException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = DeleteValidator.CheckForErrors(id);

            if (validationResult is not null)
                return new DeleteResult { Error = validationResult };

            try
            {
                var result = await _repository.DeleteAsync(id, cancellationToken);

                return new DeleteResult { Data = result };
            }
            catch (NewOrderNotFoundException ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetAllResult> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByEstablishmentAsync(id, cancellationToken);
            
            return new GetAllResult { Data = _parser.Parse(result) };
        }
    }
}
