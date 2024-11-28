using Coffee_Ecommerce.API.Features.ApiAccess.Business;
using Coffee_Ecommerce.API.Features.FooterItem.Business;
using Coffee_Ecommerce.API.Features.FooterItem.Create;
using Coffee_Ecommerce.API.Features.FooterItem.Update;
using Coffee_Ecommerce.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace Coffee_Ecommerce.API.Features.FooterItem
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterItemController : ControllerBase
    {
        private readonly IFooterItemBusiness _business;
        private readonly IApiAccessBusiness _apiAccessBusiness;

        public FooterItemController(IFooterItemBusiness business, IApiAccessBusiness apiAccessBusiness)
        {
            _business = business;
            _apiAccessBusiness = apiAccessBusiness;
        }

        [HttpPost]
        [Authorize("Bearer", Roles = "business_owner")]
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
        [Authorize("Bearer", Roles = "business_owner")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Authorize("Bearer", Roles = "business_owner,frontend")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

            if (role == "frontend")
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
        [Authorize("Bearer", Roles = "business_owner")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpPut]
        [Authorize("Bearer", Roles = "business_owner")]
        public async Task<IActionResult> Update([FromBody] UpdateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            var result = await _business.UpdateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
