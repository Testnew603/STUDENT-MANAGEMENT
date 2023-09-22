using BridgeOn_Review.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace BridgeOn_Review.Model
{    
    public class LeaveModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int DaysOfLeave { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string ApprovedBy { get; set; }

        public int StudentId { get; set; }

        public int MentorId { get; set; }

        public int BatchId { get; set; }

        public string? TokenResult { get; set; }
    }
}
