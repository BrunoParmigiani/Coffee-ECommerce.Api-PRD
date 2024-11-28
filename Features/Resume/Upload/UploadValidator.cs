using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Resume.Upload
{
    public static class UploadValidator
    {
        public static ApiError? CheckForErrors(UploadCommand command)
        {
            if (command.UserId == Guid.Empty)
                return new ApiError("Id cannot be empty");

            return null;
        }
    }
}
