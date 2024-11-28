using Coffee_Ecommerce.API.Features.ApiAccess.Business;
using Coffee_Ecommerce.API.Features.Establishment.Business;
using Coffee_Ecommerce.API.Features.Establishment.Create;
using Coffee_Ecommerce.API.Features.Establishment.PasswordChange;
using Coffee_Ecommerce.API.Features.Establishment.Update;
using Coffee_Ecommerce.API.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace Coffee_Ecommerce.API.Features.Establishment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class EstablishmentController : ControllerBase
    {
        private readonly IEstablishmentBusiness _business;
        private readonly IApiAccessBusiness _apiAccessBusiness;

        public EstablishmentController(IEstablishmentBusiness business, IApiAccessBusiness apiAccessBusiness)
        {
            _business = business;
            _apiAccessBusiness = apiAccessBusiness;
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

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "business_owner,com_module")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

            if (role == "com_module")
            {
                string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var validationResult = await _apiAccessBusiness.ValidateKeyAsync(token, cancellationToken);

                if (validationResult.Error != null)
                    return BadRequest(validationResult);

                var result = await _business.GetAllAsync(cancellationToken);
                return Ok(result);
            }
            else if (role == Roles.Owner)
            {
                var result = await _business.GetAllAsync(cancellationToken);
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "business_owner,commercial_admin")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Route("getinfo")]
        [Authorize(Roles = "commercial_place,blocked_account")]
        public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

            var result = await _business.GetByIdAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place")]
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
        [Authorize(Roles = "business_owner,commercial_place")]
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
