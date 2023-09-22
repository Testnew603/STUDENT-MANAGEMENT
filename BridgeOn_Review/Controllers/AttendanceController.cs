using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService ?? throw new ArgumentNullException(nameof(IAttendanceService));
        }

        ////GET api/AttendanceModel
        //[HttpGet]
        //public async Task<List<AttendanceModel>> GetAllAttendance()
        //{
        //    return await _attendanceService.GetAllAttendance();
        //}

        ////GET api/AttendanceModel/{1}
        //[HttpGet("Id")]
        //public async Task<ActionResult<AttendanceModel>> GetAttendanceById(int attendanceId)
        //{
        //    var result = await _attendanceService.GetAttendanceByID(attendanceId);
        //    if (result == null) { return NotFound(); }
        //    return Ok(result);
        //}

        ////POST api/AddAttendance
        //[HttpPost]
        //public async Task<AttendanceModel> AddAttendance(AttendanceModel attendanceModel)
        //{
        //    await _attendanceService.AddAttendance(attendanceModel);
        //    return attendanceModel;
        //}

        ////UPDATE api/updateAttendance/{Id}
        //[HttpPut]
        //public async Task<AttendanceModel> UpdateAttendanceById(int attendanceId, AttendanceModel attendance)
        //{
        //   var result = await _attendanceService.UpdateAttendanceById(attendanceId, attendance);
        //   return result;
        //}

        ////DELETE api/DeleteAttendanceById
        //[HttpDelete]
        //public async Task<int> DeleteAttendanceByID(int attendanceId)
        //{
        //    await _attendanceService.DeleteAttendance(attendanceId);            
        //    return (attendanceId);
        //}
        
    }
}
