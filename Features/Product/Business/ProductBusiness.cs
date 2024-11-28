using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Product.Create;
using Coffee_Ecommerce.API.Features.Product.Delete;
using Coffee_Ecommerce.API.Features.Product.Exceptions;
using Coffee_Ecommerce.API.Features.Product.GetAll;
using Coffee_Ecommerce.API.Features.Product.GetByEstablishment;
using Coffee_Ecommerce.API.Features.Product.GetById;
using Coffee_Ecommerce.API.Features.Product.Repository;
using Coffee_Ecommerce.API.Features.Product.Update;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Product.Business
{
    public sealed class ProductBusiness : IProductBusiness
    {
        private readonly IProductRepository _repository;

        public ProductBusiness(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            var entity = new ProductEntity
            {
                CreatorId = sanitizedCommand.CreatorId,
                Name = sanitizedCommand.Name,
                Price = sanitizedCommand.Price,
                Description = sanitizedCommand.Description,
                EstablishmentId = sanitizedCommand.EstablishmentId
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
            catch (ProductNotFoundException ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);

            return new GetAllResult { Data = result };
        }

        public async Task<GetByEstablishmentResult> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = GetByEstablishmentValidator.CheckForErrors(id);

            if (validationResult != null)
                return new GetByEstablishmentResult { Error = validationResult };

            try
            {
                var result = await _repository.GetByEstablishmentAsync(id, cancellationToken);
             
                return new GetByEstablishmentResult { Data = result };
            }
            catch (EstablishmentNotFoundException ex)
            {
                return new GetByEstablishmentResult { Error = new ApiError(ex.Message) };
            }
        }
        /*
         * I want to die so bad man, who cares about the so called "survivors"?
         * You suffer every. single. day. to the point you decide to just end it all
         * and after you're dead you're considered a selfish person. That's not fair.
         * 
         * If suicide is rejecting the life God gave you, I would kill myself a billion times.
         * And it wouldn't be enough for me. Perhaps I'm selfish. But who cares? Does it even matter?
        */
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
            catch (ProductNotFoundException ex)
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

            var entity = new ProductEntity
            {
                Id = sanitizedCommand.Id,
                CreatorId = sanitizedCommand.CreatorId,
                Name = sanitizedCommand.Name,
                Price = sanitizedCommand.Price,
                Description = sanitizedCommand.Description,
                EstablishmentId = sanitizedCommand.EstablishmentId
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
            catch (ProductNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ArgumentException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (EstablishmentNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
        }
    }
}
