using Coffee_Ecommerce.API.Features.Product.Business;
using Coffee_Ecommerce.API.Features.Product.Create;
using Coffee_Ecommerce.API.Features.Product.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductBusiness _business;

        public ProductController(IProductBusiness business)
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

        [HttpGet]
        [Route("establishment/{id}")]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place,customer")]
        public async Task<IActionResult> GetByEstablishment(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByEstablishmentAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "business_owner,commercial_admin,commercial_place")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

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