using Coffee_Ecommerce.API.Features.Administrator.Exceptions;
using Coffee_Ecommerce.API.Features.Establishment.Block;
using Coffee_Ecommerce.API.Features.Establishment.Create;
using Coffee_Ecommerce.API.Features.Establishment.Delete;
using Coffee_Ecommerce.API.Features.Establishment.DTO;
using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Establishment.GetAll;
using Coffee_Ecommerce.API.Features.Establishment.GetById;
using Coffee_Ecommerce.API.Features.Establishment.PasswordChange;
using Coffee_Ecommerce.API.Features.Establishment.Repository;
using Coffee_Ecommerce.API.Features.Establishment.Update;
using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Services.Interfaces;
using Coffee_Ecommerce.API.Shared.Models;
using ValuesAlreadyInUseException = Coffee_Ecommerce.API.Features.Establishment.Exceptions.ValuesAlreadyInUseException;

namespace Coffee_Ecommerce.API.Features.Establishment.Business
{
    public sealed class EstablishmentBusiness : IEstablishmentBusiness
    {
        private readonly IEstablishmentRepository _repository;
        private readonly IAddressService _addressService;
        private readonly EstablishmentParser _parser;

        public EstablishmentBusiness(IEstablishmentRepository repository, IAddressService addressService)
        {
            _repository = repository;
            _addressService = addressService;
            _parser = new EstablishmentParser();
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            var addressValidationResult = await _addressService.ValidatePostalCode(sanitizedCommand.PostalCode);

            try
            {
                if (addressValidationResult == null)
                    return new CreateResult { Error = new ApiError("Invalid address") };
            }
            catch (Exception ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }

            var entity = new EstablishmentEntity
            {
                Name = sanitizedCommand.Name,
                Email = sanitizedCommand.Email,
                Address = addressValidationResult.Results[0].FormatedAddress,
                CNPJ = sanitizedCommand.CNPJ,
                AdministratorId = sanitizedCommand.AdministratorId,
                PhoneNumber = sanitizedCommand.PhoneNumber,
                Password = sanitizedCommand.Password,
                Role = Roles.Establishment,
                Complement = sanitizedCommand.Complement
            };
            
            try
            {
                var result = await _repository.CreateAsync(entity, cancellationToken);

                return new CreateResult { Data = _parser.Parse(result) };
            }
            catch (ArgumentNullException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (ValuesAlreadyInUseException ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
            catch (AdministratorNotFoundException ex)
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
            catch (Exception ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);

            return new GetAllResult{ Data = _parser.Parse(result) };
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
            catch (EstablishmentNotFoundException ex)
            {
                return new GetByIdResult{ Error = new ApiError(ex.Message) };
            }
        }

        public async Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = UpdateValidator.Sanitize(command);
            var validationResult = UpdateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new UpdateResult { Error = validationResult };

            var addressValidationResult = await _addressService.ValidatePostalCode(sanitizedCommand.PostalCode);

            try
            {
                if (addressValidationResult == null)
                    return new UpdateResult { Error = new ApiError("Invalid address") };
            }
            catch (Exception ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }

            var entity = new EstablishmentEntity
            {
                Id = sanitizedCommand.Id,
                Name = sanitizedCommand.Name,
                Email = sanitizedCommand.Email,
                Address = addressValidationResult.Results[0].FormatedAddress,
                CNPJ = sanitizedCommand.CNPJ,
                AdministratorId = sanitizedCommand.AdministratorId,
                PhoneNumber = sanitizedCommand.PhoneNumber,
                Role = Roles.Establishment,
                Complement = sanitizedCommand.Complement
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
            catch (EstablishmentNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ValuesAlreadyInUseException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (AdministratorNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<BlockResult> BlockAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = BlockValidator.CheckForErrors(id);

            if (validationResult != null)
                return new BlockResult { Error = validationResult };

            try
            {
                var result = await _repository.BlockAsync(id, cancellationToken);

                if (!result.Item1)
                    return new BlockResult { Error = new ApiError("Could not execute the operation") };

                return new BlockResult { Data = result };
            }
            catch (Exception ex)
            {
                return new BlockResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<PasswordChangeResult> ChangePasswordAsync(ChangeCredentials credentials, CancellationToken cancellationToken)
        {
            var sanitizedCredentials = PasswordChangeValidator.Sanitize(credentials);
            var validationResult = PasswordChangeValidator.CheckForErrors(sanitizedCredentials);

            if (validationResult != null)
                return new PasswordChangeResult { Error = validationResult };

            try
            {
                var result = await _repository.ChangePasswordAsync(credentials, cancellationToken);

                return new PasswordChangeResult { Data = result };
            }
            catch (EstablishmentNotFoundException ex)
            {
                return new PasswordChangeResult { Error = new ApiError(ex.Message) };
            }
        }

    }
}
