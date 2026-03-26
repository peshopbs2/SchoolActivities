using Microsoft.EntityFrameworkCore;
using SchoolActivities.Business.Repositories.Interfaces;
using SchoolActivities.Business.Services.Interfaces;
using SchoolActivities.Models.Domain.Entities;
using SchoolActivities.Models.Dtos.Student;

namespace SchoolActivities.Business.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentService(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        public async Task<StudentResponseDto> CreateAsync(StudentRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            Student student = new Student
            {
                Id = dto.StudentId ?? Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            await _studentRepository.AddAsync(student).ConfigureAwait(false);
            await _studentRepository.CommitAsync().ConfigureAwait(false);

            return await GetByIdAsync(student.Id).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid studentId)
        {
            Student? student = await _studentRepository.GetByIdAsync(studentId).ConfigureAwait(false);

            if (student is null)
            {
                throw new KeyNotFoundException($"Student with id '{studentId}' was not found.");
            }

            _studentRepository.Remove(student);
            await _studentRepository.CommitAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
        {
            List<Student> students = await _studentRepository
                .Query()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Activity)
                .ToListAsync()
                .ConfigureAwait(false);

            return students.Select(MapToResponseDto);
        }

        public async Task<StudentResponseDto> GetByIdAsync(Guid studentId)
        {
            Student? student = await _studentRepository
                .Query()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Activity)
                .FirstOrDefaultAsync(s => s.Id == studentId)
                .ConfigureAwait(false);

            if (student is null)
            {
                throw new KeyNotFoundException($"Student with id '{studentId}' was not found.");
            }

            return MapToResponseDto(student);
        }

        public async Task<StudentResponseDto?> UpdateAsync(Guid studentId, StudentRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            Student? student = await _studentRepository
                .Query()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Activity)
                .FirstOrDefaultAsync(s => s.Id == studentId)
                .ConfigureAwait(false);

            if (student is null)
            {
                return null;
            }

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;

            _studentRepository.Update(student);
            await _studentRepository.CommitAsync().ConfigureAwait(false);

            return MapToResponseDto(student);
        }

        private static StudentResponseDto MapToResponseDto(Student student)
        {
            return new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Enrollments = student.Enrollments.Select(e => new StudentEnrollmentResponseDto
                {
                    ActivityId = e.ActivityId,
                    ActivityName = e.Activity?.Name ?? string.Empty,
                    EnrolledAtUtc = e.EnrolledAtUtc,
                    IsActive = e.IsActive
                }).ToList()
            };
        }
    }
}