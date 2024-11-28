using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.ApiAccess.ValidateKey
{
    public static class ValidateKeyValidator
    {
        public static ApiError? CheckForErrors(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new ApiError("Key cannot be empty");

            return null;
        }
    }
}
