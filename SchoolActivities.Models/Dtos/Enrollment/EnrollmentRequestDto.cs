using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Models.Dtos.Enrollment
{
    public class EnrollmentRequestDto
    {
        public Guid StudentId { get; set; }
        public Guid ActivityId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
