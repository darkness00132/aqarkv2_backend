using Application.DTOs.Reviews;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<Review> GetReviewByIdAsync(int id);

        Task<List<Review>> GetReviewsByBrokerSlug(string slug);

        Task CreateReviewAsync(CreateReview dto);

        Task UpdateReviewAsync(int id ,UpdateReview dto);

        Task DeleteReviewAsync(int id);
    }
}
