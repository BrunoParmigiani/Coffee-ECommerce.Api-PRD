using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Order.Update
{
    public sealed class UpdateValidator
    {
        public static ApiError? CheckForErrors(UpdateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            if (command.DeliveryTime < 0)
                return new ApiError("Invalid delivery time");

            return null;
        }
    }
}
