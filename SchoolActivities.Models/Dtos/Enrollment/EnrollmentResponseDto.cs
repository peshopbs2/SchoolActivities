using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Models.Dtos.Enrollment
{
    public class EnrollmentResponseDto
    {
        public Guid StudentId { get; set; }
        public string StudentFirstName { get; set; } = string.Empty;
        public string StudentLastName { get; set; } = string.Empty;

        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;

        public DateTime EnrolledAtUtc { get; set; }
        public bool IsActive { get; set; }
    }
}
