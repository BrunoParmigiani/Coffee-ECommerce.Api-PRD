using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.ApiAccess.RefreshKey
{
    public static class RefreshKeyValidator
    {
        public static ApiError? CheckForErrors(Guid id, string key)
        {
            if (id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            if (string.IsNullOrWhiteSpace(key))
                return new ApiError("Key cannot be empty");

            return null;
        }
    }
}
