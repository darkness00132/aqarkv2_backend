using Application.DTOs.Reviews;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Brokers;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Brokers;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IBrokerReviewRepo _brokerReviewRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ReviewService(IBrokerReviewRepo brokerReviewRepo, IMapper mapper , IUnitOfWork uow)
        {
            _brokerReviewRepo = brokerReviewRepo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task CreateReviewAsync(CreateReview dto)
        {
            var review = _mapper.Map<BrokerReview>(dto);
            await _brokerReviewRepo.AddAsync(review);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _brokerReviewRepo.DeleteAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var review = await _brokerReviewRepo.GetByIdAsync(id);
            return _mapper.Map<Review>(review);
        }

        public async Task<List<Review>> GetReviewsByBrokerSlug(string slug)
        {
            var reviews = await _brokerReviewRepo.GetByBrokerSlugAsync(slug);
            return _mapper.Map<List<Review>>(reviews);
        }

        public async Task UpdateReviewAsync(int id,UpdateReview dto)
        {
            var review = await _brokerReviewRepo.GetByIdForMutationAsync(id);
            if (review is null) throw ApiException.NotFound("هذه المراجعة غير موجودة");
            review.Rating = dto.Rating ?? review.Rating;
            review.Comment = dto.Comment ?? review.Comment;
            await _uow.SaveChangesAsync();
        }
    }
}
