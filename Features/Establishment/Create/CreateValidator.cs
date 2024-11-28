using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Establishment.Create
{
    public static class CreateValidator
    {
        public static ApiError? CheckForErrors(CreateCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                return new ApiError("Name cannot be empty");

            if (command.Name.Length > 100 || command.Name.Length < 3)
                return new ApiError("Name must be 3 - 100 characters");

            if (!EmailService.EmailIsValid(command.Email, CommercialConfigurations.Domain))
                return new ApiError("Email is not valid");

            if (string.IsNullOrWhiteSpace(command.PostalCode))
                return new ApiError("Postal code cannot be empty");

            if (!CnpjService.CnpjIsValid(command.CNPJ))
                return new ApiError("Invalid CNPJ");

            if (command.AdministratorId == Guid.Empty)
                return new ApiError("AdministratorId cannot be empty");

            if (!PhoneService.NumberIsValid(command.PhoneNumber))
                return new ApiError("Phone number is invalid");

            if (string.IsNullOrWhiteSpace(command.Password))
                return new ApiError("Invalid password");

            if (string.IsNullOrWhiteSpace(command.Complement))
                return new ApiError("Complement cannot be empty");

            if (command.Complement.Length > 100)
                return new ApiError("Complement cannot exceed 100 characters");

            return null;
        }

        public static CreateCommand Sanitize(CreateCommand command)
        {
            return new CreateCommand
            {
                Name = command.Name.Trim(),
                Email = command.Email.Trim(),
                PostalCode = command.PostalCode.Trim(),
                CNPJ = CnpjService.FormatCnpj(command.CNPJ),
                AdministratorId = command.AdministratorId,
                PhoneNumber = PhoneService.FormatPhoneNumber(command.PhoneNumber),
                Password = command.Password.Trim(),
                Complement = command.Complement.Trim(),
            };
        }
    }
}
