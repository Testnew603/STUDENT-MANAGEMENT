using System.ComponentModel.DataAnnotations;

namespace BridgeOn_Review.DTOs.ReviewerDTO
{
    public class ReviewerModelDTO
    {
        public string? Name { get; set; }

        [Phone]
        [RegularExpression(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)")]
        [MaxLength(10)]
        public string? ContactNo { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int DomainId { get; set; }
    }
}
