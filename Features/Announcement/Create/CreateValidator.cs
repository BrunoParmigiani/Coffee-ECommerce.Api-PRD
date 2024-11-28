using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Announcement.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            if (command.CreatorId == Guid.Empty)
                return new ApiError("Creator cannot be empty");

            if (string.IsNullOrWhiteSpace(command.Name))
                return new ApiError("Name cannot be empty");

            if (command.Name.Length > 100)
                return new ApiError("Name cannot exceed 100 characters");

            if (string.IsNullOrWhiteSpace(command.Message))
                return new ApiError("Message cannot be empty");

            if (!command.Recipients.Any())
                return new ApiError("Recipient cannot be empty");

            if (command.Recipients.Any(kvp => kvp.Key < 0 || kvp.Key > 3))
                return new ApiError("Invalid recipients");

            return null;
        }

        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                CreatorId = command.CreatorId,
                Name = command.Name.Trim(),
                Message = command.Message.Trim(),
                Recipients = command.Recipients
            };
        }
    }
}
