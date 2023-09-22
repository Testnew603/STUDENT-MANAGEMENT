using BridgeOn_Review.DTOs.ProjectDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Mentor;
using BridgeOn_Review.Services.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly IMentorServices _mentorServices;
        private readonly IStudentServices _studentServices;

        public MentorController(IMentorServices mentorServices, IStudentServices studentServices)
        {
            _mentorServices = mentorServices;
            _studentServices = studentServices;
        }
       

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> GetLogData1(Users users)
        {
            var result = await _mentorServices.LoginMentor(users);
            if (result.Message == "Login Success...!")
            {
                var token = _mentorServices.GetToken(users);
                return Ok(new { token = token, result.Message });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }


        [Authorize]
        [HttpGet("GetAllReview")]
        public async Task<List<ReviewModel>> GetAllReview()
        {
            var idClaim = User.FindFirstValue("Username");
            return await _mentorServices.GetReviewAll(idClaim);
        }

        [Authorize]
        [HttpGet("GetReviewById")]
        public async Task<List<ReviewModel>> GetReviewById(int reviewId)
        {
            var idClaim = User.FindFirstValue("Username");
            return await _mentorServices.GetReviewById(reviewId, idClaim);
        }

        [Authorize]
        [HttpPost("AddReviewDetails")]
        public async Task<ActionResult<ReviewModelMentorDTO>> AddReviewDetails(ReviewModelMentorDTO reviewModelMentorDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _mentorServices.AddReview(reviewModelMentorDTO, idClaim);
            return Ok(reviewModelMentorDTO);
        }

        [Authorize]
        [HttpPatch("UpdateReviewById")]
        public async Task<ActionResult> UpdateReviewById(int reviewId, JsonPatchDocument<ReviewModelMentorDTO> JSONreviewModelMentorDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _mentorServices.UpdateReviewById(reviewId, JSONreviewModelMentorDTO, idClaim);
            return Ok(JSONreviewModelMentorDTO);
        }

        [Authorize]
        [HttpPut("PostponedReviewDateById")]
        public async Task<ReviewModelDTO> PostponedReviewDateByID(int reviewId, ReviewModelDTO reviewModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _mentorServices.PostponedReviewDateById(reviewId, reviewModelDTO, idClaim);
            return reviewModelDTO;
        }

        [Authorize]
        [HttpGet("GetAllProject")]
        public async Task<List<ProjectModel>> GetAllProject()
        {
            var idClaim = User.FindFirstValue("Username");
            return await _mentorServices.GetProjectAll(idClaim);
        }

        [Authorize]
        [HttpGet("GetProjectById")]
        public async Task<List<ProjectModel>> GetProjectById(int projectId)
        {
            var idClaim = User.FindFirstValue("Username");
            return await _mentorServices.GetProjectById(projectId, idClaim);
        }


        [Authorize]
        [HttpPut("ProjectStatusUpdateById")]
        public async Task<ProjectModelMentorDTO> ProjectStatusUpdateById(int projectId, ProjectModelMentorDTO projectModelMentorDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _mentorServices.ProjectStatusUpdateById(projectId, projectModelMentorDTO, idClaim);
            return projectModelMentorDTO;
        }

       
    }
}
