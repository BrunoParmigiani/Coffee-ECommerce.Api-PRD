using Coffee_Ecommerce.API.Features.ApiAccess.Create;
using Coffee_Ecommerce.API.Features.ApiAccess.Delete;
using Coffee_Ecommerce.API.Features.ApiAccess.GetAll;
using Coffee_Ecommerce.API.Features.ApiAccess.GetById;
using Coffee_Ecommerce.API.Features.ApiAccess.RefreshKey;
using Coffee_Ecommerce.API.Features.ApiAccess.RemoveKey;
using Coffee_Ecommerce.API.Features.ApiAccess.Repository;
using Coffee_Ecommerce.API.Features.ApiAccess.Validate;
using Coffee_Ecommerce.API.Features.ApiAccess.ValidateKey;
using Coffee_Ecommerce.API.Services.Interfaces;
using Coffee_Ecommerce.API.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Coffee_Ecommerce.API.Features.ApiAccess.Business
{
    public sealed class ApiAccessBusiness : IApiAccessBusiness
    {
        private readonly IApiAccessRepository _repository;
        private readonly ITokenService _tokenService;

        public ApiAccessBusiness(IApiAccessRepository repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public async Task<CreateResult> CreateAsync(string serviceName, CancellationToken cancellationToken)
        {
            var validationResult = CreateValidator.CheckForErrors(serviceName);

            if (validationResult != null)
                return new CreateResult { Error = validationResult };

            Guid id = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, serviceName),
                new Claim(ClaimTypes.Role, serviceName),
                new Claim("ServiceId", id.ToString())
            };

            var apiKey = _tokenService.GenerateApiKey(claims);

            var entity = new ApiAccessEntity
            {
                Id = id,
                ServiceName = serviceName,
                Key = apiKey,
            };

            try
            {
                var result = await _repository.CreateAsync(entity, cancellationToken);

                return new CreateResult { Data = new ApiAccessEntity{ Id = id, ServiceName = serviceName, Key = apiKey } };
            }
            catch (Exception ex)
            {
                return new CreateResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = DeleteValidator.CheckForErrors(id);

            if (validationResult != null)
                return new DeleteResult { Error = validationResult };

            try
            {
                var result = await _repository.DeleteAsync(id, cancellationToken);

                return new DeleteResult { Data = result };
            }
            catch (Exception ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);

            return new GetAllResult { Data = result };
        }

        public async Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = GetByIdValidator.CheckForErrors(id);

            if (validationResult != null)
                return new GetByIdResult { Error = validationResult };

            try
            {
                var result = await _repository.GetByIdAsync(id, cancellationToken);

                return new GetByIdResult { Data = result };
            }
            catch (Exception ex)
            {
                return new GetByIdResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<RefreshKeyResult> RefreshKeyAsync(Guid entityId, string key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RemoveKeyResult> RemoveKeyAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = RemoveKeyValidator.CheckForErrors(id);

            if (validationResult != null)
                return new RemoveKeyResult { Error = validationResult };

            try
            {
                var result = await _repository.RemoveKeyAsync(id, cancellationToken);

                return new RemoveKeyResult { Data = result };
            }
            catch (Exception ex)
            {
                return new RemoveKeyResult { Error = new ApiError(ex.Message) };
            }
        }

        public async Task<ValidateKeyResult> ValidateKeyAsync(string key, CancellationToken cancellationToken)
        {
            var validationResult = ValidateKeyValidator.CheckForErrors(key);

            if (validationResult != null)
                return new ValidateKeyResult { Error = validationResult };

            var result = await _repository.ValidateKeyAsync(key, cancellationToken);

            if (!result)
                return new ValidateKeyResult { Error = new ApiError("Invalid token") };

            return new ValidateKeyResult { Data = result };
        }
    }
}
