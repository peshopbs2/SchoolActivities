using SchoolActivities.Business.Repositories.Interfaces;
using SchoolActivities.Business.Services.Interfaces;
using SchoolActivities.Models.Domain.Entities;
using SchoolActivities.Models.Dtos.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Business.Services.Implementation
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Activity> _activityRepository;

        public ActivityService(IRepository<Activity> activityRepository)
        {
            _activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
        }

        public async Task<ActivityResponseDto> CreateAsync(ActivityRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            Activity activity = new Activity
            {
                Id = dto.ActivityId ?? Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description
            };

            await _activityRepository.AddAsync(activity).ConfigureAwait(false);
            await _activityRepository.CommitAsync().ConfigureAwait(false);

            return MapToResponseDto(activity);
        }

        public async Task DeleteAsync(Guid activityId)
        {
            Activity? activity = await _activityRepository.GetByIdAsync(activityId).ConfigureAwait(false);

            if (activity is null)
            {
                throw new KeyNotFoundException($"Activity with id '{activityId}' was not found.");
            }

            _activityRepository.Remove(activity);
            await _activityRepository.CommitAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetAllAsync()
        {
            IEnumerable<Activity> activities = await _activityRepository.GetAllAsync().ConfigureAwait(false);
            return activities.Select(MapToResponseDto);
        }

        public async Task<ActivityResponseDto> GetByIdAsync(Guid activityId)
        {
            Activity? activity = await _activityRepository.GetByIdAsync(activityId).ConfigureAwait(false);

            if (activity is null)
            {
                throw new KeyNotFoundException($"Activity with id '{activityId}' was not found.");
            }

            return MapToResponseDto(activity);
        }

        public async Task<ActivityResponseDto?> UpdateAsync(Guid activityId, ActivityRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            Activity? activity = await _activityRepository.GetByIdAsync(activityId).ConfigureAwait(false);

            if (activity is null)
            {
                return null;
            }

            activity.Name = dto.Name;
            activity.Description = dto.Description;

            _activityRepository.Update(activity);
            await _activityRepository.CommitAsync().ConfigureAwait(false);

            return MapToResponseDto(activity);
        }

        private static ActivityResponseDto MapToResponseDto(Activity activity)
        {
            return new ActivityResponseDto
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description
            };
        }
    }
}
