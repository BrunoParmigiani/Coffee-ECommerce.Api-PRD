using Coffee_Ecommerce.API.Features.Administrator.Business;
using Coffee_Ecommerce.API.Features.Administrator.Create;
using Coffee_Ecommerce.API.Features.Administrator.PasswordChange;
using Coffee_Ecommerce.API.Features.Administrator.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorBusiness _business;

        public AdministratorController(IAdministratorBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        [Authorize(Roles = "business_owner")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            var result = await _business.CreateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "business_owner")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "business_owner")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _business.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "business_owner")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Route("getinfo")]
        [Authorize(Roles = "business_owner,commercial_admin,blocked_account")]
        public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

            var result = await _business.GetByIdAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "business_owner,commercial_admin")]
        public async Task<IActionResult> Update([FromBody] UpdateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            var result = await _business.UpdateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Route("block/{id}")]
        [Authorize(Roles = "business_owner")]
        public async Task<IActionResult> BlockAccount(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.BlockAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [Route("changepassword")]
        [Authorize(Roles = "business_owner,commercial_admin")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeCredentials credentials, CancellationToken cancellationToken)
        {
            if (credentials == null)
                return BadRequest("Invalid request");

            var result = await _business.ChangePasswordAsync(credentials, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
