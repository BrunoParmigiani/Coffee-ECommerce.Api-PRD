using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Establishment.GetById
{
    public static class GetByIdValidator
    {
        public static ApiError? CheckForErrors(Guid id)
        {
            if (id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            return null;
        }
    }
}
