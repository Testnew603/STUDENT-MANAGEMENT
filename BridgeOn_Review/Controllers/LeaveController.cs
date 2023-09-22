using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Leave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveServices _leaveServices;

        public LeaveController(ILeaveServices leaveServices)
        {
            _leaveServices = leaveServices;
        }

        ////GET api/getAllLeave
        //[HttpGet]
        //public async Task<List<LeaveModel>> GetAllLeave()
        //{
        //    return await _leaveServices.GetLeaveAll();
        //}

        ////GET api/getLeave/{Id}
        //[HttpGet("Id")]
        //public async Task<ActionResult<LeaveModel>> GetLeaveById(int leaveId)
        //{
        //    var result = await _leaveServices.GetLeaveById(leaveId);
        //    if (result == null) { return NotFound(); }
        //    return Ok(result);
        //}

        ////POST api/addLeave
        //[Authorize]
        //[HttpPost]
        //public async Task<LeaveModel> AddLeave(LeaveModel leaveModel)
        //{                       
        //    var idClaim = User.FindFirstValue("Username");            
        //    leaveModel.TokenResult = idClaim.ToString();            
        //    await _leaveServices.AddLeave(leaveModel);
        //    return leaveModel;
        //}

        ////UPDATE api/updateLeave/{id}
        //[HttpPut]
        //public async Task<LeaveModel> UpdateLeaveById(int leaveId, LeaveModel leaveModel)
        //{
        //    await _leaveServices.UpdateLeaveById(leaveId, leaveModel);
        //    return leaveModel;
        //}

        ////DELETE api/deleteLeave/{id}
        //[HttpDelete]
        //public async Task<int> DeleteLeaveById(int leaveId)
        //{
        //    await _leaveServices.DeleteLeave(leaveId);
        //    return leaveId;
        //}
    }
}
