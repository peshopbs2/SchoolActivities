namespace SchoolActivities.Models.Dtos.Student
{
    public class StudentEnrollmentResponseDto
    {
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public DateTime EnrolledAtUtc { get; set; }
        public bool IsActive { get; set; }
    }
}