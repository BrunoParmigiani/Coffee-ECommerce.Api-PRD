using Coffee_Ecommerce.API.Features.User.Business;
using Coffee_Ecommerce.API.Features.User.Create;
using Coffee_Ecommerce.API.Features.User.PasswordChange;
using Coffee_Ecommerce.API.Features.User.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Coffee_Ecommerce.API.Features.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            var result = await _userBusiness.CreateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var result = await _userBusiness.DeleteAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "blocked")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _userBusiness.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "customer,commercial_place,commercial_admin")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _userBusiness.GetByIdAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Route("getinfo")]
        [Authorize(Roles = "customer,suspended_account")]
        public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

            var result = await _userBusiness.GetByIdAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Route("recover")]
        [Authorize(Roles = "suspended_account")]
        public async Task<IActionResult> RecoverAccount(CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

            var result = await _userBusiness.RecoverAccountAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Update([FromBody] UpdateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            command.Id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

            var result = await _userBusiness.UpdateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeCredentials credentials, CancellationToken cancellationToken)
        {
            if (credentials == null)
                return BadRequest("Invalid request");

            var result = await _userBusiness.ChangePasswordAsync(credentials, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
