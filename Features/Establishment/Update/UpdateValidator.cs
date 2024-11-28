using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Establishment.Update
{
    public static class UpdateValidator
    {
        public static ApiError? CheckForErrors(UpdateCommand command)
        {
            if (command.Id == Guid.Empty)
                return new ApiError("Id cannot be empty");

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

            if (string.IsNullOrWhiteSpace(command.Complement))
                return new ApiError("Complement cannot be empty");

            if (command.Complement.Length > 100)
                return new ApiError("Complement cannot exceed 100 characters");

            return null;
        }

        public static UpdateCommand Sanitize(UpdateCommand command)
        {
            return new UpdateCommand
            {
                Id = command.Id,
                Name = command.Name.Trim(),
                Email = command.Email.Trim(),
                PostalCode = command.PostalCode.Trim(),
                CNPJ = CnpjService.FormatCnpj(command.CNPJ),
                AdministratorId = command.AdministratorId,
                PhoneNumber = PhoneService.FormatPhoneNumber(command.PhoneNumber),
                Complement = command.Complement.Trim(),
            };
        }
    }
}
