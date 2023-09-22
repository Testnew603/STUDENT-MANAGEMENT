namespace BridgeOn_Review.Model
{
    public class AttendanceModel
    {
        public int Id { get; set; }
        public string? StudentName{ get; set; }
        public DateTime? EntryTime { get; set; }
        public string? LateReason { get; set; }
        public DateTime? ExitTime { get; set; }
        public string? LeavingReason { get; set; }
        public string? Status { get; set; }
        public int? StudentId { get; set; }
        public int? MentorId { get; set; }
    }
}
