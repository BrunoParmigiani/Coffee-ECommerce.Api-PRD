using Coffee_Ecommerce.API.Features.NewOrder.Business;
using Coffee_Ecommerce.API.Features.NewOrder.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.NewOrder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class NewOrderController : ControllerBase
    {
        private readonly INewOrderBusiness _business;

        public NewOrderController(INewOrderBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            var result = await _business.CreateAsync(command, cancellationToken);

            if (result.Error is not null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "commercial_place")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            if (result.Error is not null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("establishment")]
        [Authorize(Roles = "commercial_place")]
        public async Task<IActionResult> GetByEstablishment(CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var result = await _business.GetByEstablishmentAsync(id, cancellationToken);

            if (result.Error is not null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
