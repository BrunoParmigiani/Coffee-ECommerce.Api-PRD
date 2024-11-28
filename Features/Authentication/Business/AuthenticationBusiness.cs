using Coffee_Ecommerce.API.Features.Administrator;
using Coffee_Ecommerce.API.Features.Administrator.Repository;
using Coffee_Ecommerce.API.Features.Authentication.RefreshToken;
using Coffee_Ecommerce.API.Features.Authentication.RevokeToken;
using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Coffee_Ecommerce.API.Features.Establishment;
using Coffee_Ecommerce.API.Features.Establishment.Repository;
using Coffee_Ecommerce.API.Features.User;
using Coffee_Ecommerce.API.Features.User.Repository;
using Coffee_Ecommerce.API.Services;
using Coffee_Ecommerce.API.Services.Interfaces;
using Coffee_Ecommerce.API.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Coffee_Ecommerce.API.Features.Authentication.Business
{
    public sealed class AuthenticationBusiness : IAuthenticationBusiness
    {
        private const string DATE_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
        private TokenConfiguration _configuration;
        private IAdministratorRepository _adminRepository;
        private IEstablishmentRepository _establishmentRepository;
        private IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationBusiness(TokenConfiguration configuration, IAdministratorRepository adminRepository, IEstablishmentRepository establishmentRepository, IUserRepository userRepository, ITokenService tokenService)
        {
            _configuration = configuration;
            _adminRepository = adminRepository;
            _establishmentRepository = establishmentRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<SigninResult> Signin(Credentials credentials, CancellationToken cancellationToken)
        {
            UserBase user = await ValidateCredentialsAsync(credentials, cancellationToken);

            if (user == null)
                return new SigninResult { Error = new ApiError("Could not authenticate user") };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            if (user is UserEntity)
            {
                claims.Add(new Claim("Address", (user as UserEntity).Address));
            }
            else if (user is EstablishmentEntity)
            {
                claims.Add(new Claim("Address", (user as EstablishmentEntity).Address));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(_configuration.DaysToExpire);

            UserBase refreshedUser = await RefreshInfoAsync(user, cancellationToken);

            if (refreshedUser == null)
                return new SigninResult { Error = new ApiError("Could not authenticate user") };

            DateTime createDate = DateTime.UtcNow;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            Token token = new Token (
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );

            return new SigninResult { Data = token };
        }

        public async Task<RefreshTokenResult> RefreshToken(Token token, CancellationToken cancellationToken)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var email = principal.Identity.Name;

            UserBase user = await ValidateCredentialsAsync(email, cancellationToken);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.UtcNow)
                return new RefreshTokenResult { Error = new ApiError("Invalid request") };

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            UserBase refreshedUser = await RefreshInfoAsync(user, cancellationToken);

            if (refreshedUser == null)
                return new RefreshTokenResult { Error = new ApiError("Invalid request") };

            DateTime createDate = DateTime.UtcNow;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            Token newToken = new Token (
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );

            return new RefreshTokenResult { Data = newToken };
        }

        public async Task<RevokeTokenResult> RevokeToken(string email, CancellationToken cancellationToken)
        {
            UserBase user = await ValidateCredentialsAsync(email, cancellationToken);

            if (user == null)
                return new RevokeTokenResult { Error = new ApiError("Invalid request") };

            try
            {
                if (user is AdministratorEntity)
                {
                    await _adminRepository.RevokeTokenAsync(user.Email, cancellationToken);
                    return new RevokeTokenResult { Data = true };
                }
                else if (user is EstablishmentEntity)
                {
                    await _establishmentRepository.RevokeTokenAsync(user.Email, cancellationToken);
                    return new RevokeTokenResult { Data = true };
                }
                else if (user is UserEntity)
                {
                    await _userRepository.RevokeTokenAsync(user.Email, cancellationToken);
                    return new RevokeTokenResult { Data = true };
                }

                return new RevokeTokenResult { Error = new ApiError("Invalid request") };
            }
            catch (Exception ex)
            {
                return new RevokeTokenResult { Error = new ApiError(ex.Message) };
            }
        }

        private async Task<UserBase> ValidateCredentialsAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            if (EmailService.EmailIsValid(credentials.Email, CommercialConfigurations.Domain))
            {
                var admin = await _adminRepository.ValidateCredentialsAsync(credentials, cancellationToken);

                if (admin != null)
                    return admin;

                var establishment = await _establishmentRepository.ValidateCredentialsAsync(credentials, cancellationToken);

                if (establishment != null)
                    return establishment;

                return null;
            }
            else if (EmailService.EmailIsValid(credentials.Email))
            {
                var user = await _userRepository.ValidateCredentialsAsync(credentials, cancellationToken);

                if (user != null)
                    return user;

                return null;
            }

            return null;
        }

        private async Task<UserBase> ValidateCredentialsAsync(string email, CancellationToken cancellationToken)
        {
            if (EmailService.EmailIsValid(email, CommercialConfigurations.Domain))
            {
                var admin = await _adminRepository.ValidateCredentialsAsync(email, cancellationToken);

                if (admin != null)
                    return admin;

                var establishment = await _establishmentRepository.ValidateCredentialsAsync(email, cancellationToken);

                if (establishment != null)
                    return establishment;

                return null;
            }
            else if (EmailService.EmailIsValid(email))
            {
                var user = await _userRepository.ValidateCredentialsAsync(email, cancellationToken);

                if (user != null)
                    return user;

                return null;
            }

            return null;
        }

        private async Task<UserBase> RefreshInfoAsync(UserBase user, CancellationToken cancellationToken)
        {
            if (user is AdministratorEntity)
            {
                return await _adminRepository.RefreshInfoAsync((AdministratorEntity)user, cancellationToken);
            }
            else if (user is EstablishmentEntity)
            {
                return await _establishmentRepository.RefreshInfoAsync((EstablishmentEntity)user, cancellationToken);
            }
            else if (user is UserEntity)
            {
                return await _userRepository.RefreshInfoAsync((UserEntity)user, cancellationToken);
            }

            return null;
        }
    }
}
