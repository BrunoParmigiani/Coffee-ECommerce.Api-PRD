using System.Security.Claims;

namespace Coffee_Ecommerce.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateApiKey(IEnumerable<Claim> claims);
    }
}
