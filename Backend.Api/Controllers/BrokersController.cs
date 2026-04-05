using Application.Constants;
using Application.DTOs.Brokers;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokersController : ControllerBase
    {
        private readonly IBrokerService _brokerService;

        public BrokersController(IBrokerService brokerService)
        {
            _brokerService = brokerService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBrokers(CancellationToken ct)
        {
            return Ok(await _brokerService.GetAllBrokers(ct));
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBrokerBySlug(string slug,CancellationToken ct)
        {
            return Ok(await _brokerService.GetBrokerBySlug(slug, ct));
        }

        [Authorize(Policy = Policies.BrokerOnly)]
        [HttpGet("me")]
        public async Task<ActionResult<Broker>> GetMyBrokerProfile(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw ApiException.Unauthorized();

            return await _brokerService.GetMyBrokerProfile(parsedUserId);  
        }

        [Authorize(Policy = Policies.BrokerOnly)]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateBrokerProfile(UpdateBrokerProfile dto) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw ApiException.Unauthorized();

            await _brokerService.UpdateBrokerBrofile(parsedUserId, dto);

            return Ok();
        }
    }
}
