using Coffee_Ecommerce.API.Features.Order.Create;
using Coffee_Ecommerce.API.Features.Order.Business;
using Microsoft.AspNetCore.Mvc;
using Coffee_Ecommerce.API.Features.Order.Update;
using Microsoft.AspNetCore.Authorization;
using Coffee_Ecommerce.API.Features.Order.Rate;
using Coffee_Ecommerce.API.Features.Order.GetPage;

namespace Coffee_Ecommerce.API.Features.Order
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBusiness _business;

        public OrderController(IOrderBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        [Authorize(Roles = "commercial_place")]
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
        [Authorize(Roles = "blocked")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "commercial_admin,commercial_place")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _business.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "commercial_admin,commercial_place,customer")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Route("page")]
        [Authorize(Roles = "commercial_admin,commercial_place")]
        public async Task<IActionResult> GetPage([FromQuery] GetPageCommand command, CancellationToken cancellationToken)
        {
            var result = await _business.GetPageAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "commercial_place")]
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
        [Route("rate")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Rate([FromBody] RateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            var result = await _business.RateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetUserOrders([FromQuery] GetPageCommand command, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            command.CustomerId = userId;

            var result = await _business.GetPageAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
