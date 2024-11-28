using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Administrator.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            // Name validation ok
            if (string.IsNullOrWhiteSpace(command.Name))
                return new ApiError("Name cannot be empty");

            if (command.Name.Length > 100 || command.Name.Length < 3)
                return new ApiError("Name must have 3 - 100 characters");

            // Email validation ok, for now
            if (!EmailService.EmailIsValid(command.Email, CommercialConfigurations.Domain))
                return new ApiError($"Invalid email");

            // CPF validation ok
            if (!CpfService.CpfIsValid(command.CPF))
                return new ApiError("CPF is invalid");

            // Password validation ok, for now
            if (!PhoneService.NumberIsValid(command.PhoneNumber))
                return new ApiError("Phone number is invalid");

            if (string.IsNullOrWhiteSpace(command.Password))
                return new ApiError("Password cannot be empty");

            if (string.IsNullOrWhiteSpace(command.Role))
                return new ApiError("Invalid role");

            if (command.Role != Roles.Owner && command.Role != Roles.Administrator)
                return new ApiError("Invalid role");

            return null;
        }

        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                Name = command.Name.Trim(),
                Email = command.Email.Trim(),
                CPF = CpfService.FormatCpf(command.CPF),
                Password = command.Password.Trim(),
                PhoneNumber = PhoneService.FormatPhoneNumber(command.PhoneNumber),
                Role = command.Role.Trim(),
                EstablishmentId = command.EstablishmentId
            };
        }
    }
}
