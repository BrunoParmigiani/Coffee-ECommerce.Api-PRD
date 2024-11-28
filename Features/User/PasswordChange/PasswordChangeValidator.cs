using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.User.PasswordChange
{
    public static class PasswordChangeValidator
    {
        public static ApiError? CheckForErrors(ChangeCredentials credentials)
        {
            if (!EmailService.EmailIsValid(credentials.Email))
                return new ApiError("Invalid email");

            if (string.IsNullOrWhiteSpace(credentials.CurrentPassword))
                return new ApiError("Current password does not match");

            if (string.IsNullOrWhiteSpace(credentials.NewPassword))
                return new ApiError("New password is invalid");

            if (string.IsNullOrWhiteSpace(credentials.ConfirmNewPassword))
                return new ApiError("Confirmation password is invalid");

            if (credentials.NewPassword != credentials.ConfirmNewPassword)
                return new ApiError("Confirmation password does not match new password");

            return null;
        }

        public static ChangeCredentials Sanitize(ChangeCredentials credentials)
        {
            return new ChangeCredentials
            {
                Email = credentials.Email.Trim(),
                CurrentPassword = credentials.CurrentPassword.Trim(),
                NewPassword = credentials.NewPassword.Trim(),
                ConfirmNewPassword = credentials.ConfirmNewPassword.Trim()
            };
        }
    }
}
