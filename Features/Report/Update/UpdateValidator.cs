using Coffee_Ecommerce.API.Features.Report.Enums;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Report.Update
{
    public static class UpdateValidator
    {
        internal static ApiError? CheckForErrors(UpdateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            if (command.Status < 0 || command.Status > Enum.GetValues(typeof(Statuses)).Length - 1)
                return new ApiError("Invalid status");

            return null;
        }

        public static UpdateCommand Sanitize(UpdateCommand command)
        {
            return new UpdateCommand
            {
                Id = command.Id,
                Status = command.Status,
            };
        }
    }
}
