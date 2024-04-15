using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.CurriculumDtos;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Curriculum;
using Prime.Progreso.Domain.Pagination.Technology;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ICurriculumRepository _curriculumRepo;
        private IValidationProvider _validationProvider;
        private readonly ITechnologyRepository _technologyRepo;

        public CurriculumService(ICurriculumRepository curriculumRepo, IValidationProvider validationProvider, ITechnologyRepository technologyRepo)
        {
            _curriculumRepo = curriculumRepo;
            _validationProvider = validationProvider;
            _technologyRepo = technologyRepo;
        }

        public async Task<CurriculumResponseDto> CreateAsync(CurriculumRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!await _technologyRepo.HasAnyAsync(dto.TechnologyId))
            {
                throw new NotFoundException("Technology was not found!");
            }

            return await _curriculumRepo.CreateAsync<CurriculumRequestDto, CurriculumResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _curriculumRepo.DeleteAsync(id);
        }

        public async Task<CurriculumResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _curriculumRepo.GetByIdAsync<CurriculumResponseDto>(id);
        }

        public async Task<PaginatedResult<CurriculumResponseDto>> GetPageAsync(CurriculumsPagingInfo pagingInfo)
        {
            if (pagingInfo.TechnologyFilter != string.Empty)
            {
                var technologies = await _technologyRepo
                    .GetPageByFilterAsync(new TechnologiesPagingInfo() { PartOfNameFilter = pagingInfo.TechnologyFilter });

                pagingInfo.technologyIds = technologies.Content.Select(t => t.Id).ToList();

                if(!pagingInfo.technologyIds.Any())
                {
                    return PaginatedResult<CurriculumResponseDto>.EmptyResult(0, 0);
                }
            }

            return await _curriculumRepo.GetPageByFilterAsync<CurriculumResponseDto>(pagingInfo);
        }

        public async Task UpdateAsync(Guid id, CurriculumRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _validationProvider.TryValidateAsync(dto);

            if (!await _technologyRepo.HasAnyAsync(dto.TechnologyId))
            {
                throw new NotFoundException("Technology was not found!");
            }

            await _curriculumRepo.UpdateAsync(id, dto);
        }
    }
}
