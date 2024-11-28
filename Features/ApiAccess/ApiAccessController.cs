using Coffee_Ecommerce.API.Features.ApiAccess.Business;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.ApiAccess
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccessController : ControllerBase
    {
        private readonly IApiAccessBusiness _business;

        public ApiAccessController(IApiAccessBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string serviceName, CancellationToken cancellationToken)
        {
            var result = await _business.CreateAsync(serviceName, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _business.GetAllAsync(cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetByIdAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RemoveKey(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.RemoveKeyAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
