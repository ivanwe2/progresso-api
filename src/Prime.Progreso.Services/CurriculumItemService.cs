using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Services
{
    public class CurriculumItemService : ICurriculumItemService
    {
        private readonly ICurriculumItemRepository _curriculumItemRepo;
        private IValidationProvider _validationProvider;
        private readonly IActivityRepository _activityRepository;
        private readonly ICurriculumRepository _curriculumRepository;

        public CurriculumItemService(ICurriculumItemRepository curriculumItemRepo,
                                     IValidationProvider validationProvider,
                                     IActivityRepository activityRepository,
                                     ICurriculumRepository curriculumRepository)
        {
            _curriculumItemRepo = curriculumItemRepo;
            _validationProvider = validationProvider;
            _activityRepository = activityRepository;
            _curriculumRepository = curriculumRepository;
        }

        public async Task<CurriculumItemResponseDto> CreateAsync(CurriculumItemRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await ValidateRequestDto(dto);

            return await _curriculumItemRepo.CreateAsync<CurriculumItemRequestDto, CurriculumItemResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _curriculumItemRepo.DeleteAsync(id);
        }

        public async Task<CurriculumItemResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _curriculumItemRepo.GetByIdAsync<CurriculumItemResponseDto>(id);
        }

        public async Task<PaginatedResult<CurriculumItemResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<CurriculumItemResponseDto, bool>> filter = null)
        {
            return await _curriculumItemRepo.GetPageAsync(pageNumber, pageSize, filter);
        }

        public async Task UpdateAsync(Guid id, CurriculumItemRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _validationProvider.TryValidateAsync(dto);

            await ValidateRequestDto(dto);

            await _curriculumItemRepo.UpdateAsync(id, dto);
        }

        private async Task ValidateRequestDto(CurriculumItemRequestDto dto)
        {
            var activityExists = await _activityRepository.HasAnyAsync(dto.ActivityId);

            if (!activityExists)
            {
                throw new NotFoundException("Invalid activity ID.");
            }

            var curriculumExists = await _curriculumRepository.HasAnyAsync(dto.CurriculumId);

            if (!curriculumExists)
            {
                throw new NotFoundException("Invalid curriculum ID.");
            }
        }
    }
}
