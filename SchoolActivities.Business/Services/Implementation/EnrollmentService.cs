using Microsoft.EntityFrameworkCore;
using SchoolActivities.Business.Repositories.Interfaces;
using SchoolActivities.Business.Services.Interfaces;
using SchoolActivities.Models.Domain.Entities;
using SchoolActivities.Models.Dtos.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Business.Services.Implementation
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Activity> _activityRepository;

        public EnrollmentService(
            IRepository<Enrollment> enrollmentRepository,
            IRepository<Student> studentRepository,
            IRepository<Activity> activityRepository)
        {
            _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
        }

        public async Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            Student? student = await _studentRepository.GetByIdAsync(dto.StudentId).ConfigureAwait(false);
            if (student is null)
            {
                throw new KeyNotFoundException($"Student with id '{dto.StudentId}' was not found.");
            }

            Activity? activity = await _activityRepository.GetByIdAsync(dto.ActivityId).ConfigureAwait(false);
            if (activity is null)
            {
                throw new KeyNotFoundException($"Activity with id '{dto.ActivityId}' was not found.");
            }

            Enrollment? existingEnrollment = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .FirstOrDefaultAsync(
                    e => e.StudentId == dto.StudentId && e.ActivityId == dto.ActivityId)
                .ConfigureAwait(false);

            if (existingEnrollment is not null)
            {
                throw new InvalidOperationException("This student is already enrolled in the selected activity.");
            }

            Enrollment enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                ActivityId = dto.ActivityId,
                IsActive = dto.IsActive,
                EnrolledAtUtc = DateTime.UtcNow
            };

            await _enrollmentRepository.AddAsync(enrollment).ConfigureAwait(false);
            await _enrollmentRepository.CommitAsync().ConfigureAwait(false);

            Enrollment createdEnrollment = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .FirstAsync(
                    e => e.StudentId == dto.StudentId && e.ActivityId == dto.ActivityId)
                .ConfigureAwait(false);

            return MapToResponseDto(createdEnrollment);
        }

        public async Task DeleteAsync(Guid studentId, Guid activityId)
        {
            Enrollment? enrollment = await _enrollmentRepository
                .Query()
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.ActivityId == activityId)
                .ConfigureAwait(false);

            if (enrollment is null)
            {
                throw new KeyNotFoundException(
                    $"Enrollment for student '{studentId}' and activity '{activityId}' was not found.");
            }

            _enrollmentRepository.Remove(enrollment);
            await _enrollmentRepository.CommitAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync()
        {
            List<Enrollment> enrollments = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> GetByActivityIdAsync(Guid activityId)
        {
            List<Enrollment> enrollments = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .Where(e => e.ActivityId == activityId)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> GetByStudentIdAsync(Guid studentId)
        {
            List<Enrollment> enrollments = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .Where(e => e.StudentId == studentId)
                .ToListAsync()
                .ConfigureAwait(false);

            return enrollments.Select(MapToResponseDto);
        }

        public async Task<EnrollmentResponseDto?> UpdateAsync(Guid studentId, Guid activityId, EnrollmentRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            Enrollment? enrollment = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.ActivityId == activityId)
                .ConfigureAwait(false);

            if (enrollment is null)
            {
                return null;
            }

            enrollment.IsActive = dto.IsActive;

            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.CommitAsync().ConfigureAwait(false);

            Enrollment updatedEnrollment = await _enrollmentRepository
                .Query()
                .Include(e => e.Student)
                .Include(e => e.Activity)
                .FirstAsync(e => e.StudentId == studentId && e.ActivityId == activityId)
                .ConfigureAwait(false);

            return MapToResponseDto(updatedEnrollment);
        }

        private static EnrollmentResponseDto MapToResponseDto(Enrollment enrollment)
        {
            return new EnrollmentResponseDto
            {
                StudentId = enrollment.StudentId,
                StudentFirstName = enrollment.Student.FirstName,
                StudentLastName = enrollment.Student.LastName,
                ActivityId = enrollment.ActivityId,
                ActivityName = enrollment.Activity.Name,
                EnrolledAtUtc = enrollment.EnrolledAtUtc,
                IsActive = enrollment.IsActive
            };
        }
    }
}
