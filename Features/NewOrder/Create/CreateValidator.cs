using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.NewOrder.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            if (command.UserId == Guid.Empty)
                return new ApiError("User id cannot be empty");

            if (command.PaymentMethod < 0 || command.PaymentMethod > 1)
                return new ApiError("Invalid payment method");

            if (command.EstablishmentId == Guid.Empty)
                return new ApiError("Establishment id cannot be empty");

            if (!command.Items.Any())
                return new ApiError("Items cannot be empty");

            if (string.IsNullOrWhiteSpace(command.UserName))
                return new ApiError("Username cannot be empty");

            if (string.IsNullOrWhiteSpace(command.UserName))
                return new ApiError("User address cannot be empty");

            if (string.IsNullOrWhiteSpace(command.UserName))
                return new ApiError("User complement cannot be empty");

            return null;
        }

        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                Id = command.Id,
                UserId = command.UserId,
                PaymentMethod = command.PaymentMethod,
                EstablishmentId = command.EstablishmentId,
                Items = command.Items,
                UserName = command.UserName.Trim(),
                UserAddress = command.UserAddress.Trim(),
                UserComplement = command.UserComplement.Trim()
            };
        }
    }
}
