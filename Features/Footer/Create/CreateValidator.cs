using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.FooterItem.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
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

        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                Name = command.Name.Trim(),
                Link = command.Link.Trim()
            };
        }
    }
}
