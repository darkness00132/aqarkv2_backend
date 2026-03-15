using Application.DTOs.Ad;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdsController(IAdService adService)
        {
            _adService = adService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AdListItemDTO>>> GetAllAds(CancellationToken ct)
        {
            return await _adService.GetAllAds(ct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdDTO>> GetAd(string slug,CancellationToken ct)
        {
            return await _adService.GetAdBySlug(slug,ct);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAd([FromForm]CreateAdDTO request, CancellationToken ct) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw ApiException.Unauthorized();

            await _adService.CreateAdAsync(request, parsedUserId, ct);
            return Created();
        }
    }
}
