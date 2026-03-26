using SchoolActivities.Models.Dtos.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Business.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync();
        Task<IEnumerable<EnrollmentResponseDto>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<EnrollmentResponseDto>> GetByActivityIdAsync(Guid activityId);
        Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto dto);
        Task<EnrollmentResponseDto?> UpdateAsync(Guid studentId, Guid activityId, EnrollmentRequestDto dto);
        Task DeleteAsync(Guid studentId, Guid activityId);
    }
}
