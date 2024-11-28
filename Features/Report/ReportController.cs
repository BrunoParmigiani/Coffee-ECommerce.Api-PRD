using Coffee_Ecommerce.API.Features.Report.Business;
using Coffee_Ecommerce.API.Features.Report.Create;
using Coffee_Ecommerce.API.Features.Report.GetPage;
using Coffee_Ecommerce.API.Features.Report.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ReportController : ControllerBase
    {
        private readonly IReportBusiness _business;

        public ReportController(IReportBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            command.UserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
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

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetUserReports([FromQuery] GetPageCommand command, CancellationToken cancellationToken)
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
