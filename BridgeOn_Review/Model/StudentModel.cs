﻿using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace BridgeOn_Review.Model
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address { get; set; }
        public string? Qualification { get; set; }
        [Phone]
        [RegularExpression(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)")]
        [MaxLength(10)]
        public string? ContactNumber{ get; set; }
        [EmailAddress]
        public string? EmailId { get; set; }
        public int? BatchId { get; set; }
        public int? MentorId { get; set; }
        public int? AdvisorId { get; set; }
        public int? DomainId { get; set; }
        public string?Status { get; set; }
    }
}
