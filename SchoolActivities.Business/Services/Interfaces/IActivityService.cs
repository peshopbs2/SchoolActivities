using SchoolActivities.Models.Dtos.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolActivities.Business.Services.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityResponseDto>> GetAllAsync();
        Task<ActivityResponseDto> GetByIdAsync(Guid activityId);
        Task<ActivityResponseDto> CreateAsync(ActivityRequestDto dto);
        Task<ActivityResponseDto?> UpdateAsync(Guid activityId, ActivityRequestDto dto);
        Task DeleteAsync(Guid activityId);
    }
}
