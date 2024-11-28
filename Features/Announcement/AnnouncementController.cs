using Coffee_Ecommerce.API.Features.Announcement.Business;
using Coffee_Ecommerce.API.Features.Announcement.Create;
using Coffee_Ecommerce.API.Features.Announcement.Update;
using Coffee_Ecommerce.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coffee_Ecommerce.API.Features.Announcement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementBusiness _business;

        public AnnouncementController(IAnnouncementBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            command.CreatorId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var result = await _business.CreateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place,customer")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _business.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetByRole(CancellationToken cancellationToken)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value);
            
            var result = await _business.GetByRoleAsync(role, id, cancellationToken);

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

            command.CreatorId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var result = await _business.UpdateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
