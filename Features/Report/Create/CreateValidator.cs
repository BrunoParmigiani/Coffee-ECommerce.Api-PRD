using Coffee_Ecommerce.API.Features.Report.Enums;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Report.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            if (command.UserId == Guid.Empty)
                return new ApiError("UserId cannot be empty");

            if (command.EstablishmentId == Guid.Empty)
                return new ApiError("EstablishmentId cannot be empty");

            if (command.OrderId == Guid.Empty)
                return new ApiError("OrderId cannot be empty");

            if (command.Problem < 0 || command.Problem > Enum.GetValues(typeof(Problems)).Length - 1)
                return new ApiError("Invalid problem");
            
            if (string.IsNullOrWhiteSpace(command.Description))
                return new ApiError("Description cannot be empty");

            return null;
        }

        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                UserId = command.UserId,
                EstablishmentId = command.EstablishmentId,
                OrderId = command.OrderId,
                Problem = command.Problem,
                Description = command.Description.Trim()
            };
        }
    }
}