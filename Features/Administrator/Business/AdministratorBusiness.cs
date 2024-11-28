using Coffee_Ecommerce.API.Features.Administrator.Block;
using Coffee_Ecommerce.API.Features.Administrator.Create;
using Coffee_Ecommerce.API.Features.Administrator.Delete;
using Coffee_Ecommerce.API.Features.Administrator.DTO;
using Coffee_Ecommerce.API.Features.Administrator.Exceptions;
using Coffee_Ecommerce.API.Features.Administrator.GetAll;
using Coffee_Ecommerce.API.Features.Administrator.GetById;
using Coffee_Ecommerce.API.Features.Administrator.PasswordChange;
using Coffee_Ecommerce.API.Features.Administrator.Repository;
using Coffee_Ecommerce.API.Features.Administrator.Update;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Administrator.Business
{
    public sealed class AdministratorBusiness : IAdministratorBusiness
    {
        private readonly IAdministratorRepository _repository;
        private readonly AdministratorParser _parser;

        public AdministratorBusiness(IAdministratorRepository repository)
        {
            _repository = repository;
            _parser = new AdministratorParser();
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            var entity = new AdministratorEntity
            {
                Name = sanitizedCommand.Name,
                Email = sanitizedCommand.Email,
                CPF = sanitizedCommand.CPF,
                Password = sanitizedCommand.Password,
                PhoneNumber = sanitizedCommand.PhoneNumber,
                Role = command.Role,
                EstablishmentId = sanitizedCommand.EstablishmentId
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
                return new DeleteResult{ Error = validationResult };

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

            return new GetAllResult { Data = _parser.Parse(result) };
        }

        public async Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = GetByIdValidator.CheckForErrors(id);

            if (validationResult != null)
                return new GetByIdResult{ Error = validationResult };

            try
            {
                var result = await _repository.GetByIdAsync(id, cancellationToken);

                return new GetByIdResult { Data = _parser.Parse(result) };
            }
            catch (AdministratorNotFoundException ex)
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

            var entity = new AdministratorEntity
            {
                Id = sanitizedCommand.Id,
                Name = sanitizedCommand.Name,
                Email = sanitizedCommand.Email,
                CPF = sanitizedCommand.CPF,
                PhoneNumber = sanitizedCommand.PhoneNumber,
                EstablishmentId = sanitizedCommand.EstablishmentId
            };

            try
            {
                var result = await _repository.UpdateAsync(entity, cancellationToken);

                return new UpdateResult { Data = _parser.Parse(result) };
            }
            catch (ArgumentNullException ex)
            {
                return new UpdateResult{ Error = new ApiError(ex.Message) };
            }
            catch (AdministratorNotFoundException ex)
            {
                return new UpdateResult { Error = new ApiError(ex.Message) };
            }
            catch (ValuesAlreadyInUseException ex)
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
            catch (Exception ex)
            {
                return new PasswordChangeResult { Error = new ApiError(ex.Message) };
            }
        }
    }
}
