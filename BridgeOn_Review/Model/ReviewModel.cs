namespace BridgeOn_Review.Model
{
    public class ReviewModel
    {
        public int? Id { get; set; }
        public int? WeekNo { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public DateTime? PostponedDate { get; set; }
        public string? Decisions { get; set; }

        public double? TaskMarks { get; set; }
        public int? ReviewType { get; set; }

        public string? ReviewMode { get; set; }

        public string? Status { get; set; }

        public string? StatusDescription { get; set; }

        public int? StudentId { get; set; }

        public int? MentorId { get; set; }

        public int? ReviewerId { get; set; }

        public int? DomainId { get; set; }
    }
}
