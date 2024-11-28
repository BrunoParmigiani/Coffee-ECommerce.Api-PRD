using Coffee_Ecommerce.API.Features.Resume.Business;
using Coffee_Ecommerce.API.Features.Resume.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.Resume
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeBusiness _business;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(IResumeBusiness business, ILogger<ResumeController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UploadResume([FromBody] UploadCommand command, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            command.UserId = userId;

            var result = await _business.UploadFileAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "business_owner,commercial_admin,customer")]
        public async Task<IActionResult> DeleteResume(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteFileAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
        
        [HttpGet]
        [Authorize(Roles = "business_owner,commercial_admin")]
        public async Task<IActionResult> GetAllResumes(CancellationToken cancellationToken)
        {
            var result = await _business.GetAllFilesAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "business_owner,commercial_admin,customer")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.GetFileAsync(id, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
