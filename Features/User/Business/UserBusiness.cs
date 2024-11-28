using Coffee_Ecommerce.API.Features.User.Create;
using Coffee_Ecommerce.API.Features.User.Delete;
using Coffee_Ecommerce.API.Features.User.DTO;
using Coffee_Ecommerce.API.Features.User.Exceptions;
using Coffee_Ecommerce.API.Features.User.GetAll;
using Coffee_Ecommerce.API.Features.User.GetById;
using Coffee_Ecommerce.API.Features.User.PasswordChange;
using Coffee_Ecommerce.API.Features.User.Recovery;
using Coffee_Ecommerce.API.Features.User.Repository;
using Coffee_Ecommerce.API.Features.User.Update;
using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Services.Interfaces;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.User.Business
{
    public sealed class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _repository;
        private readonly IAddressService _addressService;
        private readonly UserParser _parser;

        public UserBusiness(IUserRepository repository, IAddressService addressService)
        {
            _repository = repository;
            _addressService = addressService;
            _parser = new UserParser();
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

            var entity = new UserEntity
            {
                Name = sanitizedCommand.Name,
                Email = sanitizedCommand.Email,
                CPF = sanitizedCommand.CPF,
                Password = sanitizedCommand.Password,
                PhoneNumber = sanitizedCommand.PhoneNumber,
                Address = addressValidationResult.Results[0].FormatedAddress,
                Role = Roles.User,
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
        }

        public async Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = DeleteValidator.CheckForErrors(id);

            if (validationResult != null)
                return new DeleteResult { Error = validationResult };

            try
            {
                var result = await _repository.DeleteAsync(id, cancellationToken);

                if (!result)
                    return new DeleteResult { Error = new ApiError("Account already schedule for deletion") };

                return new DeleteResult { Data = result };
            }
            catch (UserNotFoundException ex)
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
            catch (UserNotFoundException ex)
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

            var entity = new UserEntity
            {
                Id = sanitizedCommand.Id,
                Name = sanitizedCommand.Name,
                Email = sanitizedCommand.Email,
                CPF = sanitizedCommand.CPF,
                PhoneNumber = sanitizedCommand.PhoneNumber,
                Address = addressValidationResult.Results[0].FormatedAddress,
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
            catch (UserNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ValuesAlreadyInUseException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<RecoveryResult> RecoverAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = RecoveryValidator.CheckForErrors(id);

            if (validationResult != null)
                return new RecoveryResult { Error = validationResult };

            try
            {
                var result = await _repository.RecoverAccountAsync(id, cancellationToken);

                return new RecoveryResult { Data = result };
            }
            catch (Exception ex)
            {
                return new RecoveryResult { Error = new ApiError(ex.Message) };
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
            catch (UserNotFoundException ex)
            {
                return new PasswordChangeResult { Error = new ApiError(ex.Message) };
            }
        }
    }
}
