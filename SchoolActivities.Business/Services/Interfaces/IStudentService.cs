using SchoolActivities.Models.Dtos.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Business.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponseDto>> GetAllAsync();
        Task<StudentResponseDto> GetByIdAsync(Guid studentId);
        Task<StudentResponseDto> CreateAsync(StudentRequestDto dto);
        Task<StudentResponseDto?> UpdateAsync(Guid studentId, StudentRequestDto dto);
        Task DeleteAsync(Guid studentId);
    }
}
