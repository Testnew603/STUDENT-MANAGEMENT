namespace BridgeOn_Review.DTOs.LeaveDTO
{
    public class LeaveModelDTO
    {
        public DateTime Date { get; set; }

        public int DaysOfLeave { get; set; }

        public string Reason { get; set; }

        public string? TokenResult { get; set; }
    }
}
