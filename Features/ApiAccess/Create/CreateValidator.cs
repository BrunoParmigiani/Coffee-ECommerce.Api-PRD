using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.ApiAccess.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                return new ApiError("Name cannot be empty");

            if (serviceName.Length < 3 || serviceName.Length > 100)
                return new ApiError("Name must have 3 - 100 characters");

            return null;
        }
    }
}
