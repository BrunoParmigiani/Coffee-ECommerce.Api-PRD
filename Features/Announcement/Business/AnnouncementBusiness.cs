using Coffee_Ecommerce.API.Features.Announcement.Create;
using Coffee_Ecommerce.API.Features.Announcement.Delete;
using Coffee_Ecommerce.API.Features.Announcement.Exceptions;
using Coffee_Ecommerce.API.Features.Announcement.GetAll;
using Coffee_Ecommerce.API.Features.Announcement.GetById;
using Coffee_Ecommerce.API.Features.Announcement.GetByRole;
using Coffee_Ecommerce.API.Features.Announcement.Repository;
using Coffee_Ecommerce.API.Features.Announcement.Update;
using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Announcement.Business
{
    public sealed class AnnouncementBusiness : IAnnouncementBusiness
    {
        private readonly IAnnouncementRepository _repository;
        
        public AnnouncementBusiness(IAnnouncementRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = CreateValidator.Sanitize(command);
            var validationResult = CreateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            var entity = new AnnouncementEntity
            {
                CreatorId = command.CreatorId,
                Name = sanitizedCommand.Name,
                Message = sanitizedCommand.Message,
                Recipients = string.Join("", command.Recipients
                    .Where(kvp => kvp.Value)
                    .Select(kvp => kvp.Key.ToString()))
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
            catch (AnnouncementNotFoundException ex)
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
                return new GetByIdResult{ Error = validationResult };

            try
            {
                var result = await _repository.GetByIdAsync(id, cancellationToken);

                return new GetByIdResult { Data = result };
            }
            catch (AnnouncementNotFoundException ex)
            {
                return new GetByIdResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetByRoleResult> GetByRoleAsync(string role, Guid userId, CancellationToken cancellationToken)
        {
            var validationResult = GetByRoleValidator.CheckForErrors(role);

            if (validationResult != null)
                return new GetByRoleResult { Error = validationResult };

            int number = Roles.RoleNumber(role);
            var result = await _repository.GetByRoleAsync(number, userId, cancellationToken);

            return new GetByRoleResult { Data = result };
        }

        public async Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken)
        {
            var sanitizedCommand = UpdateValidator.Sanitize(command);
            var validationResult = UpdateValidator.CheckForErrors(sanitizedCommand);

            if (validationResult != null)
                return new UpdateResult { Error = validationResult };

            var entity = new AnnouncementEntity
            {
                Id = sanitizedCommand.Id,
                CreatorId = sanitizedCommand.CreatorId,
                Name = sanitizedCommand.Name,
                Message = sanitizedCommand.Message,
                Recipients = string.Join("", command.Recipients
                    .Where(kvp => kvp.Value)
                    .Select(kvp => kvp.Key.ToString()))
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
            catch (AnnouncementNotFoundException ex)
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
