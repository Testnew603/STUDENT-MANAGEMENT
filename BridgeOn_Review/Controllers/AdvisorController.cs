using BridgeOn_Review.DTOs.MentorDTO;
using BridgeOn_Review.DTOs.ReviewerDTO;
using BridgeOn_Review.DTOs.StudentDTO;
using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Advisor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private readonly IAdvisorService _advisorService;

        public AdvisorController(IAdvisorService advisorService)
        {
            _advisorService = advisorService;
        }

        //GET api/login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> GetLogData1(Users users)
        {
            var result = await _advisorService.LoginAdvisor(users);
            if (result.Message == "Login Success...!")
            {
                var token = _advisorService.GetToken(users);
                return Ok(new { token = token, result.Message });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }       

        //GET api/GetAllReviewer
        [Authorize]
        [HttpGet("GetAllReviewer")]
        public async Task<List<ReviewerModel>> GetAllReviewer()
        {
            var idClaim = User.FindFirstValue("Username");
            return await _advisorService.GetAllReviewer(idClaim);
        }

        //GET api/GetReviewer/{Id}
        [Authorize]
        [HttpGet("GetReviewerById")]
        public async Task<ActionResult<ReviewerModel>> GetReviewerById(int reviewerId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _advisorService.GetReviewerById(reviewerId, idClaim);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }

        //POST api/addReviewer
        [Authorize]
        [HttpPost("AddReviewer")]
        public async Task<ActionResult<ReviewerModelDTO>> AddReviewer(ReviewerModelDTO reviewerModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.AddReviewer(reviewerModelDTO, idClaim);
            return Ok(reviewerModelDTO);
        }

        //UPDATE api/addReviewer
        [Authorize]
        [HttpPatch("UpdateReviewerById")]
        public async Task<IActionResult> UpdateReviewerById(int reviewerId, JsonPatchDocument<ReviewerModelDTO> JSONreviewerModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.UpdateReviewerById(reviewerId, JSONreviewerModelDTO, idClaim);
            return Ok(JSONreviewerModelDTO);
        }

        //UPDATE api/deleteReviewer/{id}
        [Authorize]
        [HttpDelete("DeleteReviewerById")]
        public async Task<int> DeleteReviewerById(int reviewerId)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.DeleteReviewer(reviewerId, idClaim);
            return reviewerId;
        }


        //GET api/GetAllMentor
        [Authorize]
        [HttpGet("GetAllMentor")]
        public async Task<List<MentorModel>> GetAllMentor()
        {
            var idClaim = User.FindFirstValue("Username");
            return await _advisorService.GetMentorAll(idClaim);
        }

        //GET api/GetMentor/{Id}
        [Authorize]
        [HttpGet("GetMentorById")]
        public async Task<ActionResult<MentorModel>> GetMentorById(int mentorId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _advisorService.GetMentorById(mentorId, idClaim);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }

        //POST api/addMentor
        [Authorize]
        [HttpPost("AddMentor")]
        public async Task<ActionResult<MentorModelDTO>> AddMentor(MentorModelDTO mentorModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.AddMentor(mentorModelDTO, idClaim);
            return Ok(mentorModelDTO);
        }

        //UPDATE api/mentor
        [Authorize]
        [HttpPatch("UpdateMentorById")]
        public async Task<IActionResult> UpdateMentorById(int mentorId, JsonPatchDocument<MentorModelDTO> JSONmentorModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.UpdateMentorById(mentorId, JSONmentorModelDTO, idClaim);
            return Ok(JSONmentorModelDTO);
        }

        //DELETE api/deleteMentor/{id}
        [Authorize]
        [HttpDelete("DeleteMentorById")]
        public async Task<int> DeleteMentorById(int mentorId)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.DeleteMentorById(mentorId, idClaim);
            return mentorId;
        }

        //GET api/GetStudentAll
        [Authorize]
        [HttpGet("GetStudentAll")]
        public async Task<List<StudentModel>> GetStudentAll()
        {
            var idClaim = User.FindFirstValue("Username");
            return await _advisorService.GetStudentAll(idClaim);
        }

        //GET api/GetStudent/{Id}
        [Authorize]
        [HttpGet("GetStudentById")]
        public async Task<ActionResult<StudentModel>> GetStudentById(int studentId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _advisorService.GetStudentById(studentId, idClaim);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }

        //POST api/addStudent
        [Authorize]
        [HttpPost("AddStudent")]
        public async Task<ActionResult<StudentModelDTO>> AddStudent(StudentModelDTO studentModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.AddStudent(studentModelDTO, idClaim);
            return Ok(studentModelDTO);
        }

        //UPDATE api/mentor
        [Authorize]
        [HttpPatch("UpdateStudentById")]
        public async Task<IActionResult> UpdateStudentById(int studentId, JsonPatchDocument<StudentModelDTO> JSONstudentModelDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.UpdateStudentById(studentId, JSONstudentModelDTO, idClaim);
            return Ok(JSONstudentModelDTO);
        }

        //DELETE api/deleteMentor/{id}
        [Authorize]
        [HttpDelete("DeleteStudentById")]
        public async Task<int> DeleteStudentById(int studentId)
        {
            var idClaim = User.FindFirstValue("Username");
            await _advisorService.DeleteStudentById(studentId, idClaim);
            return studentId;
        }
    }
}
