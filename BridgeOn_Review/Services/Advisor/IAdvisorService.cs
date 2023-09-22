using BridgeOn_Review.DTOs.MentorDTO;
using BridgeOn_Review.DTOs.ReviewerDTO;
using BridgeOn_Review.DTOs.StudentDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace BridgeOn_Review.Services.Advisor
{
    public interface IAdvisorService
    {       
        Task<(List<Users>, string Message)> LoginAdvisor(Users users);

        Task<List<ReviewerModel>> GetAllReviewer(string TokenResult);

        Task<List<ReviewerModel>> GetReviewerById(int reviewerId, string TokenResult);

        Task AddReviewer(ReviewerModelDTO reviewerModelDTO, string TokenResult);

        Task UpdateReviewerById(int reviewerId, JsonPatchDocument<ReviewerModelDTO>
                                                JSONreviewerModelDTO, string TokenResult);
        Task DeleteReviewer(int reviewerId, string TokenResult);

        Task<List<MentorModel>> GetMentorAll(string TokenResult);

        Task<List<MentorModel>> GetMentorById(int mentorId, string TokenResult);

        Task AddMentor(MentorModelDTO mentorModelDTO, string TokenResult);
        Task UpdateMentorById(int mentorId, JsonPatchDocument<MentorModelDTO>
                                                   JSONmentorModelDTO, string TokenResult);
        Task DeleteMentorById(int mentorId, string TokenResult);

        Task<List<StudentModel>> GetStudentAll(string TokenResult);
        Task<List<StudentModel>> GetStudentById(int studentId, string TokenResult);
        Task AddStudent(StudentModelDTO studentModelDTO, string TokenResult);
        Task UpdateStudentById(int studentId, JsonPatchDocument<StudentModelDTO>
                                                   JSONstudentModelDTO, string TokenResult);
        Task DeleteStudentById(int studentId, string TokenResult);

        string GetToken(Users usersData);
    }
}