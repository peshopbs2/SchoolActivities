using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Models.Dtos.Student
{
    public class StudentResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<StudentEnrollmentResponseDto> Enrollments { get; set; } = new List<StudentEnrollmentResponseDto>(); 
    }
}
