using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Order.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            if (command.UserId == Guid.Empty)
                return new ApiError("UserId cannot be empty");

            if (command.PaymentMethod < 0 || command.PaymentMethod > 1)
                return new ApiError("PaymentMethod must be 0 or 1");

            if (command.EstablishmentId == Guid.Empty)
                return new ApiError("UserId cannot be empty");

            if (command.DeliveryTime < 0)
                return new ApiError("Invalid delivery time");

            if (command.Items.Count() < 1)
                return new ApiError("The order has no items");
            
            foreach (var item in command.Items)
            {
                if (string.IsNullOrWhiteSpace(item.Name) || item.Price < 1 || item.Quantity < 1)
                    return new ApiError("One or more items were invalid");
            }
            
            if (command.DeniedOrder && string.IsNullOrEmpty(command.DeniedReason))
                return new ApiError("Invalid reason");

            if (command.DeniedOrder && command.DeniedReason.Length > 500)
                return new ApiError("Reason cannot exceed 500 characters");

            return null;
        }
        
        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                UserId = command.UserId,
                PaymentMethod = command.PaymentMethod,
                EstablishmentId = command.EstablishmentId,
                DeliveryTime = command.DeliveryTime,
                Items = command.Items.Select(ItemSanitizer).ToArray(),
                DeniedOrder = command.DeniedOrder,
                DeniedReason = string.IsNullOrWhiteSpace(command.DeniedReason) ? null : command.DeniedReason.Trim()
            };
        }

        private static OrderItem ItemSanitizer(OrderItem item)
        {
            return new OrderItem
            {
                Name = item.Name.Trim(),
                Price = item.Price,
                Quantity = item.Quantity
            };
        }
    }
}
