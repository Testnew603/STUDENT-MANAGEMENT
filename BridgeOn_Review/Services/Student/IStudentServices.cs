using Azure;
using BridgeOn_Review.DTOs.AttendanceDTO;
using BridgeOn_Review.DTOs.LeaveDTO;
using BridgeOn_Review.DTOs.ProjectDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace BridgeOn_Review.Services.Student
{
    public interface IStudentServices
    {        

        Task<(List<Users>, string Message)> LoginStudent(Users users);

        Task<List<LeaveModel>> GetLeaveAll(LeaveModel leaveModel);
        
        Task AddLeave(LeaveModelDTO leaveModelDTO);
        Task UpdateLeave(int leaveId, JsonPatchDocument<LeaveModelDTO> JSONleaveModelDTO, string TokenResult);
        Task<List<AttendanceModel>> GetAllAttendance(string TokenResult);
        Task AddAttendance(AttendanceModelDTO attendanceModelDTO, string TokenResult);
        Task AddAttendanceExitTime(int attendanceId, AttendanceExitModelDTO attendanceExitModelDTO, string TokenResult);
        Task UpdateAttendanceById(int attendanceId, JsonPatchDocument<AttendanceModelDTO>
                                               JSONattendanceModelDTO, string TokenResult);

        Task<List<ReviewModel>> GetReviewAll(string TokenResult);
        Task<List<ReviewModel>> GetReviewById(int reviewId, string TokenResult);
        Task SendReviewPostponeDate(int reviewId, JsonPatchDocument<ReviewModelDTO>
                                               JSONreviewModelDTO, string TokenResult);
        Task<List<ProjectModel>> GetAllProject(string TokenResult);
        Task<List<ProjectModel>> GetProjectById(int projectId, string TokenResult);
        Task AddProject(ProjectModelDTO projectModelDTO, string TokenResult);
        Task UpdateProjectById(int projectId, JsonPatchDocument<ProjectModelDTO>
                                               JSONprojectModelDTO, string TokenResult);



        string GetToken(Users usersData);
    }
}