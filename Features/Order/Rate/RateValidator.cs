using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Order.Rate
{
    public sealed class RateValidator
    {
        public static ApiError? CheckForErrors(RateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

            if (command.TimeRating < 0 || command.TimeRating > 5)
                return new ApiError("Rating out of range (0 - 5)");

            if (command.QualityRating < 0 || command.QualityRating > 5)
                return new ApiError("Rating out of range (0 - 5)");

            if (command.UserComments != null)
            {
                if (command.UserComments.Length > 500)
                    return new ApiError("Comment cannot exceed 500 characters");
            }

            return null;
        }

        public static RateCommand Sanitize(RateCommand command)
        {
            return new RateCommand
            {
                Id = command.Id,
                TimeRating = command.TimeRating,
                QualityRating = command.QualityRating,
                UserComments = string.IsNullOrWhiteSpace(command.UserComments) ? null : command.UserComments.Trim()
            };
        }
    }
}
