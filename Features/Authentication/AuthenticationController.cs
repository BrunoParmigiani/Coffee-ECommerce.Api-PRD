using Coffee_Ecommerce.API.Features.Authentication.Business;
using Coffee_Ecommerce.API.Features.Authentication.Signin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationBusiness _authBusiness;

        public AuthenticationController(IAuthenticationBusiness authBusiness)
        {
            _authBusiness = authBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Signin([FromBody] Credentials credentials, CancellationToken cancellationToken)
        {
            if (credentials == null)
                return BadRequest("Invalid request");

            var result = await _authBusiness.Signin(credentials, cancellationToken);

            if (result.Error != null)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var result = await _authBusiness.RefreshToken(request, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Revoke(CancellationToken cancellationToken)
        {
            var email = User.Identity.Name;

            var result = await _authBusiness.RevokeToken(email, cancellationToken);

            if (!result.Data)
                return BadRequest("Invalid request");

            return NoContent();
        }
    }
}
