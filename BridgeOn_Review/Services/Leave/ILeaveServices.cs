using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Leave
{
    public interface ILeaveServices
    {
        Task<List<LeaveModel>> GetLeaveAll();

        Task<LeaveModel> GetLeaveById(int leaveId);

        Task AddLeave(LeaveModel leaveModel);

        Task<LeaveModel> UpdateLeaveById(int leaveId, LeaveModel leaveModel);

        Task DeleteLeave(int leaveId);
    }
}