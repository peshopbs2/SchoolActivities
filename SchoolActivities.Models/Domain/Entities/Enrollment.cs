using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Models.Domain.Entities
{
    public class Enrollment
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
        public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
