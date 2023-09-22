using BridgeOn_Review.DTOs.ProjectDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace BridgeOn_Review.Services.Mentor
{
    public interface IMentorServices
    {        
        Task<(List<Users>, string Message)> LoginMentor(Users users);
        Task<List<ReviewModel>> GetReviewAll(string TokenResult);
        Task<List<ReviewModel>> GetReviewById(int reviewId, string TokenResult);
        Task<string> AddReview(ReviewModelMentorDTO reviewModelMentorDTO, string TokenResult);
        Task UpdateReviewById(int reviewId, JsonPatchDocument<ReviewModelMentorDTO>
                                        JSONreviewModelMentorDTO, string TokenResult);
        Task<ReviewModelDTO> PostponedReviewDateById(int reviewId, ReviewModelDTO reviewModelDTO, string TokenResult);


        Task<List<ProjectModel>> GetProjectAll(string TokenResult);
        Task<List<ProjectModel>> GetProjectById(int projectId, string TokenResult);
        Task<ProjectModelMentorDTO> ProjectStatusUpdateById(int projectId, ProjectModelMentorDTO
                                                            projectModelMentorDTO, string TokenResult);





        string GetToken(Users usersData);
    }
}