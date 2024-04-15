using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.ActivityDtos;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private IValidationProvider _validationProvider;

        public ActivityService(IActivityRepository activityRepository,
                               IValidationProvider validationProvider)
        {
            _activityRepository = activityRepository;
            _validationProvider = validationProvider;
        }

        public async Task<ActivityResponseDto> CreateAsync(ActivityRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            return await _activityRepository.CreateAsync<ActivityRequestDto, ActivityResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _activityRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<ActivityResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<ActivityResponseDto, bool>> filter = null)
        {
            return await _activityRepository.GetPageAsync(pageNumber, pageSize, filter);
        }

        public async Task<ActivityResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _activityRepository.GetByIdAsync<ActivityResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, ActivityRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _validationProvider.TryValidateAsync(dto);

            await _activityRepository.UpdateAsync(id, dto);
        }
    }
}
