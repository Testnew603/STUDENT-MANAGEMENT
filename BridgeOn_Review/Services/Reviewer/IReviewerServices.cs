using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Reviewer
{
    public interface IReviewerServices
    {
        Task<List<ReviewerModel>> GetAllReviewer();

        Task<ReviewerModel> GetReviewerById(int reviewerId);

        Task AddReviewer(ReviewerModel reviewerModel);

        Task<ReviewerModel> UpdateReviewerById(int reviewerId, ReviewerModel reviewerModel);

        Task DeleteReviewer(int reviewerId);

        Task<ReviewModelReviewerDTO> UpdateReviewById(int reviewId, ReviewModelReviewerDTO reviewModelReviewerDTO, string TokenResult);

        Task<(List<Users>, string Message)> LoginReviewer(Users users);
        string GetToken(Users usersData);
    }
}