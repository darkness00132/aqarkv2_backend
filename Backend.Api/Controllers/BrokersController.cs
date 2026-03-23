using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
