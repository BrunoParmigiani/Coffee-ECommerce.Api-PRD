using Coffee_Ecommerce.API.Features.FooterItem.Create;
using Coffee_Ecommerce.API.Features.FooterItem.Delete;
using Coffee_Ecommerce.API.Features.FooterItem.Exceptions;
using Coffee_Ecommerce.API.Features.FooterItem.GetAll;
using Coffee_Ecommerce.API.Features.FooterItem.GetById;
using Coffee_Ecommerce.API.Features.FooterItem.Repository;
using Coffee_Ecommerce.API.Features.FooterItem.Update;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.FooterItem.Business
{
    public sealed class FooterItemBusiness : IFooterItemBusiness
    {
        private readonly IFooterItemRepository _repository;

        public FooterItemBusiness(IFooterItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            var entity = new FooterItemEntity
            {
                Name = sanitizedCommand.Name,
                Link = sanitizedCommand.Link
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
            catch (FooterItemNotFoundException ex)
            {
                return new DeleteResult{ Error = new ApiError(ex.Message) };
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
            catch (FooterItemNotFoundException ex)
            {
                return new GetByIdResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = UpdateValidator.Sanitize(command);
            var validationResult = UpdateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new UpdateResult { Error = validationResult };

            var entity = new FooterItemEntity
            {
                Id = sanitizedCommand.Id,
                Name = sanitizedCommand.Name,
                Link = sanitizedCommand.Link,
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
            catch (FooterItemNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
        }
    }
}
