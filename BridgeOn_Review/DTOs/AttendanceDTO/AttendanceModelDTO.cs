using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks.Sources;

namespace BridgeOn_Review.DTOs.AttendanceDTO
{
    public class AttendanceModelDTO
    {
        public DateTime? EntryTime { get; set; }                
        public string? LateReason { get; set; }

    }
}
