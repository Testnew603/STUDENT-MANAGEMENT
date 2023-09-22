namespace BridgeOn_Review.DTOs.ReviewDTO
{
    public class ReviewModelMentorDTO
    {
        public int WeekNo { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string ReviewMode { get; set; }
        public int ReviewType { get; set; }
        public int StudentId { get; set; }
        public int ReviewerId { get; set; }

    }
}
