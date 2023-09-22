using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Reviewer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerServices _reviewerServices;

        public ReviewerController(IReviewerServices reviewerServices)
        {
            _reviewerServices = reviewerServices;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> GetLogData1(Users users)
        {
            var result = await _reviewerServices.LoginReviewer(users);
            if (result.Message == "Login Success...!")
            {
                var token = _reviewerServices.GetToken(users);
                return Ok(new { token = token, result.Message });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        ////GET api/GetAllReviewer
        //[HttpGet]
        //public async Task<List<ReviewerModel>> GetAllReviewer()
        //{
        //    return await _reviewerServices.GetAllReviewer();
        //}

        ////GET api/GetReviewer/{Id}
        //[HttpGet("Id")]
        //public async Task<ActionResult<ReviewerModel>> GetReviewerById(int reviewerId)
        //{
        //    var result = await _reviewerServices.GetReviewerById(reviewerId);
        //    if (result == null) { return NotFound(); }
        //    return Ok(result);
        //}

        ////POST api/addReviewer
        //[HttpPost]
        //public async Task<ReviewerModel> AddReviewer(ReviewerModel reviewerModel)
        //{
        //    await _reviewerServices.AddReviewer(reviewerModel);
        //    return reviewerModel;
        //}

        ////UPDATE api/updateReviewer/{Id}
        //[HttpPut]
        //public async Task<ReviewerModel> UpdateReviewerById(int reviewerId, ReviewerModel reviewerModel)
        //{
        //    await _reviewerServices.UpdateReviewerById(reviewerId, reviewerModel);
        //    return reviewerModel;
        //}

        ////DELETE api/deleteProduct/{Id}
        //[HttpDelete]
        //public async Task<int> DeleteReviewerById(int reviewerId)
        //{
        //    await _reviewerServices.DeleteReviewer(reviewerId);
        //    return reviewerId;
        //}
         
        //UPDATE api/updateReviewer/{Id}
        [Authorize]
        [HttpPut("UpdateReviewById")]
        public async Task<ActionResult<ReviewModelReviewerDTO>> UpdateReviewById(int reviewId, ReviewModelReviewerDTO reviewModelReviewerDTO)
        {
            var idClaim = User.FindFirstValue("Username");
            await _reviewerServices.UpdateReviewById(reviewId, reviewModelReviewerDTO, idClaim);
            return Ok(reviewModelReviewerDTO);
        }
    }
}
