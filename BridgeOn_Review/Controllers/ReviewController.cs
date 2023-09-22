using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;

        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        ////GET api/viewAllReview
        //[HttpGet]
        //public async Task<List<ReviewModel>> GetReviewAll()
        //{
        //    return await _reviewServices.GetReviewAll();
        //}

        ////GET api/viewReview/{Id}
        //[HttpGet("Id")]
        //public async Task<ActionResult<ReviewModel>> GetReviewById(int reviewId)
        //{
        //    var result = await _reviewServices.GetReviewById(reviewId);
        //    if (result == null) { return NotFound(); }
        //    return Ok(result);
        //}

        ////POST api/addReview
        //[HttpPost]
        //public async Task<ReviewModel> AddReview(ReviewModel reviewModel)
        //{
        //    await _reviewServices.AddReview(reviewModel);
        //    return reviewModel;
        //}

        ////UPDATE api/updateReview
        //[HttpPut]
        //public async Task<ReviewModel> UpdateReviewById(int reviewId, ReviewModel reviewModel)
        //{
        //    await _reviewServices.UpdateReviewById(reviewId, reviewModel);  
        //    return reviewModel;
        //}

        ////DELETE api/deleteReview
        //[HttpDelete]
        //public async Task<int> DeleteReviewById(int reviewId)
        //{
        //    await _reviewServices.DeleteReview(reviewId);
        //    return reviewId;
        //}
    }
}
