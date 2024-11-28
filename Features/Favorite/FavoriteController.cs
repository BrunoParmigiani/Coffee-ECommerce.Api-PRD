using Coffee_Ecommerce.API.Features.Favorite.Business;
using Coffee_Ecommerce.API.Features.Favorite.GetByUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coffee_Ecommerce.API.Features.Favorite
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer", Roles = "customer")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteBusiness _business;

        public FavoriteController(IFavoriteBusiness business)
        {
            _business = business;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return BadRequest("Invalid request");

            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            command.UserId = userId;

            var result = await _business.CreateAsync(command, cancellationToken);

            if (result.Error != null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _business.DeleteAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUser(CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var result = await _business.GetByUserAsync(userId, cancellationToken);

            return Ok(result);
        }
    }
}
