using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Favorite.GetByUser
{
    public static class GetByUserValidator
    {
        public static ApiError? CheckForErrors(Guid userId)
        {
            if (userId == Guid.Empty)
                return new ApiError("User id cannot be empty");

            return null;
        }
    }
}