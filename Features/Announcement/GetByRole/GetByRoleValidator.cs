using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Announcement.GetByRole
{
    public static class GetByRoleValidator
    {
        public static ApiError? CheckForErrors(string role)
        {
            int number = Roles.RoleNumber(role);

            if (number < 0)
                return new ApiError("Invalid role");

            return null;
        }
    }
}
