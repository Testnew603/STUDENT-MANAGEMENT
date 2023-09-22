using Azure;
using BridgeOn_Review.DataBase;
using BridgeOn_Review.DTOs.AttendanceDTO;
using BridgeOn_Review.DTOs.LeaveDTO;
using BridgeOn_Review.DTOs.ProjectDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BridgeOn_Review.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;

        public StudentController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }

       
        //------------------------ START LOGIN ------------------------//
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> GetLogData1(Users users)
        {
            var result = await _studentServices.LoginStudent(users);
            if (result.Message == "Login Success...!")
            {
                var token = _studentServices.GetToken(users);
                return Ok(new { token = token, result.Message });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        //------------------------ END LOGIN ------------------------//


        [Authorize]
        [HttpPost("AddLeave")]
        public async Task<LeaveModelDTO> AddLeave(LeaveModelDTO leaveModelDTO)
        {
            LeaveModel leaveModel = new LeaveModel();
            var idClaim = User.FindFirstValue("Username");
            leaveModel.TokenResult = idClaim.ToString();
            await _studentServices.AddLeave(leaveModelDTO);
            return leaveModelDTO;
        }

        [Authorize]
        [HttpGet("GetLeave")]
        public async Task<List<LeaveModel>> GetLeaveDetails()
        {
            LeaveModel leaveModel = new LeaveModel();
            var idClaim = User.FindFirstValue("Username");
            leaveModel.TokenResult = idClaim.ToString();
            var result = await _studentServices.GetLeaveAll(leaveModel);
            return result.ToList();
        }

        [Authorize]
        [HttpPatch("UpdateLeave")]
        public async Task<IActionResult> UpdateLeaveById(int leaveId, JsonPatchDocument<LeaveModelDTO> JSONleaveModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.UpdateLeave(leaveId, JSONleaveModelDTO, idClaim);
            return Ok(JSONleaveModelDTO);
        }

        //------------------------ END LEAVE ------------------------//

        //------------------------ START ATTENDANCE ------------------------//

        [Authorize]
        [HttpGet("GetAttendance")]
        public async Task<List<AttendanceModel>> GetAllAttendance()
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _studentServices.GetAllAttendance(idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpPost("AddAttendance")]
        public async Task<ActionResult<AttendanceModelDTO>> AddAttendance(AttendanceModelDTO attendanceModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.AddAttendance(attendanceModelDTO, idClaim);
            return Ok(attendanceModelDTO);
        }

        [Authorize]
        [HttpPut("AddAttendanceExitTime")]
        public async Task<ActionResult<AttendanceExitModelDTO>> AddAttendanceExitTime(int attendanceId, AttendanceExitModelDTO attendanceExitModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.AddAttendanceExitTime(attendanceId, attendanceExitModelDTO, idClaim);
            return Ok(attendanceExitModelDTO);
        }

        [Authorize]
        [HttpPatch("UpdateAttendanceById")]
        public async Task<ActionResult<AttendanceModelDTO>> UpdateAttendanceById(int attendanceId, JsonPatchDocument<AttendanceModelDTO> JSONattendanceModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.UpdateAttendanceById(attendanceId, JSONattendanceModelDTO, idClaim);
            return Ok(JSONattendanceModelDTO);
        }

        //------------------------ END ATTENDANCE ------------------------//

        [Authorize]
        [HttpGet("GetAllReview")]
        public async Task<List<ReviewModel>> GetAllReview()
        {
            var idClaim = User.FindFirstValue("Username");
            return await _studentServices.GetReviewAll(idClaim);
        }

        [Authorize]
        [HttpGet("GetReviewById")]
        public async Task<List<ReviewModel>> GetReviewById(int reviewId)
        {
            var idClaim = User.FindFirstValue("Username");
            return await _studentServices.GetReviewById(reviewId, idClaim);
        }

        [Authorize]
        [HttpPatch("ReviewDatePostponedRequest")]
        public async Task<ActionResult<ReviewModelDTO>> ReviewDatePostponedRequest(int reviewId, JsonPatchDocument<ReviewModelDTO>
                                               JSONreviewModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.SendReviewPostponeDate(reviewId, JSONreviewModelDTO, idClaim);
            return Ok(JSONreviewModelDTO);
        }

        //------------------------ START PROJECT ------------------------//

        [Authorize]
        [HttpGet("GetAllProject")]
        public async Task<List<ProjectModel>> GetAllProject()
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _studentServices.GetAllProject(idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpGet("GetProjectById")]
        public async Task<List<ProjectModel>> GetProjectById(int projectId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _studentServices.GetProjectById(projectId, idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpPost("AddProject")]
        public async Task<ActionResult<ProjectModelDTO>> AddProject(ProjectModelDTO projectModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.AddProject(projectModelDTO, idClaim);
            return Ok(projectModelDTO);
        }

        [Authorize]
        [HttpPatch("UpdateProjectById")]
        public async Task<ActionResult<ProjectModelDTO>> UpdateProjectById(int projectId, JsonPatchDocument<ProjectModelDTO> JSONprojectModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _studentServices.UpdateProjectById(projectId, JSONprojectModelDTO, idClaim);
            return Ok(JSONprojectModelDTO);
        }

        //------------------------ END PROJECT ------------------------//
    }
}