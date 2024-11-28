using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.FooterItem.Update
{
    public static class UpdateValidator
    {
        public static ApiError? CheckForErrors(UpdateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            if (string.IsNullOrWhiteSpace(command.Name))
                return new ApiError("Name cannot be empty");

            if (command.Name.Length > 100)
                return new ApiError("Name cannot exceed 100 characters");

            if (string.IsNullOrWhiteSpace(command.Link))
                return new ApiError("Link cannot be empty");

            if (command.Link.Length > 255)
                return new ApiError("Link cannot exceed 255 characters");

            return null;
        }

        public static UpdateCommand Sanitize(UpdateCommand command)
        {
            return new UpdateCommand
            {
                Id = command.Id,
                Name = command.Name.Trim(),
                Link = command.Link.Trim()
            };
        }
    }
}
