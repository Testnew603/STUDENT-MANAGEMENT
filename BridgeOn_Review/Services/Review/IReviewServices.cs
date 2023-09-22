using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Review
{
    public interface IReviewServices
    {
        Task<List<ReviewModel>> GetReviewAll();

        Task<ReviewModel> GetReviewById(int reviewId);

        Task AddReview(ReviewModel reviewModel);

        Task<ReviewModel> UpdateReviewById(int reviewId, ReviewModel reviewModel);

        Task DeleteReview(int reviewId);
    }
}