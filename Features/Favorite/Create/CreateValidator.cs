using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Favorite.GetByUser
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            if (command.UserId == Guid.Empty)
                return new ApiError("User id cannot be empty");

            if (command.ProductId == Guid.Empty)
                return new ApiError("Product id cannot be empty");

            return null;
        }
    }
}