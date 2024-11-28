using Coffee_Ecommerce.API.Identity;
using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Administrator.Update
{
    public static class UpdateValidator
    {
        public static ApiError? CheckForErrors(UpdateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

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

            return null;
        }

        public static UpdateCommand Sanitize(UpdateCommand command)
        {
            return new UpdateCommand
            {
                Id = command.Id,
                Name = command.Name.Trim(),
                Email = command.Email.Trim(),
                CPF = CpfService.FormatCpf(command.CPF),
                PhoneNumber = PhoneService.FormatPhoneNumber(command.PhoneNumber),
                EstablishmentId = command.EstablishmentId
            };
        }
    }
}
