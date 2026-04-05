using Application.DTOs.Reviews;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewsController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetReviewsByBrokerSlug(string slug)
        {
            var reviews = await _reviewService.GetReviewsByBrokerSlug(slug);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }

        [HttpGet("broker/{slug}")]
        public async Task<IActionResult> GetReviewsByBrokerSlug2(string slug)
        {
            var reviews = await _reviewService.GetReviewsByBrokerSlug(slug);
            return Ok(reviews);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReview(CreateReview dto)
        {
            await _reviewService.CreateReviewAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, UpdateReview dto)
        {
            await _reviewService.UpdateReviewAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return Ok();
        }
    }
}
