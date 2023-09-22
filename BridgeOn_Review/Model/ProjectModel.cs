namespace BridgeOn_Review.Model
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Modules { get; set; }

        public string? ShortDescription { get; set; }

        public DateTime? ProposedDate { get; set; }

        public string? Status { get; set; }

        public string? Remarks { get; set; }

        public int? Review_Attended { get; set; }

        public int? Domain_Id { get; set; }

        public int? Student_Id { get; set; }

        public int? Mentor_Id { get; set; }    

    }
}
