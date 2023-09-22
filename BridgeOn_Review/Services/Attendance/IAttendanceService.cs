using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<List<AttendanceModel>> GetAllAttendance();
        Task<AttendanceModel> GetAttendanceByID(int attendanceId);
        Task AddAttendance(AttendanceModel attendance);
        Task <AttendanceModel> UpdateAttendanceById(int attendanceId, AttendanceModel attendance);
        Task DeleteAttendance(int attendanceId);
        
        
    }
}