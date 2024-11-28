using Coffee_Ecommerce.API.Features.Report.Enums;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Report.GetPage
{
    public static class GetPageValidator
    {
        public static ApiError? CheckForErrors(GetPageCommand command)
        {
            if (command.Page < 1)
                return new ApiError("Invalid page");

            if (command.Items < 10 || command.Items > 20)
                return new ApiError("Items count must be between 10 - 20");

            if (command.Status > Enum.GetValues(typeof(Statuses)).Length - 1)
                return new ApiError("Invalid status");

            return null;
        }
    }
}
