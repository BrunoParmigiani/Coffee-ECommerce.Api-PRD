using Coffee_Ecommerce.API.Features.Authentication.RefreshToken;
using Coffee_Ecommerce.API.Features.Authentication.RevokeToken;
using Coffee_Ecommerce.API.Features.Authentication.Signin;

namespace Coffee_Ecommerce.API.Features.Authentication.Business
{
    public interface IAuthenticationBusiness
    {
        public Task<SigninResult> Signin(Credentials credentials, CancellationToken cancellationToken);
        public Task<RefreshTokenResult> RefreshToken(Token token, CancellationToken cancellationToken);
        public Task<RevokeTokenResult> RevokeToken(string email, CancellationToken cancellationToken);
    }
}
