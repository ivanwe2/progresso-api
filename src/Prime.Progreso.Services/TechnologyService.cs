using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using System.Linq.Expressions;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Technology;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly ITechnologyRepository _technologyRepository;
        private IValidationProvider _validationProvider;
        private readonly ICodingChallengeRepository _codingChallengeRepository;

        public TechnologyService(ITechnologyRepository technologyRepository, 
            IValidationProvider validationProvider, 
            ICodingChallengeRepository codingChallengeRepository)
        {
            _technologyRepository = technologyRepository;
            _validationProvider = validationProvider;
            _codingChallengeRepository = codingChallengeRepository;
        }

        public async Task<TechnologyResponseDto> CreateAsync(TechnologyRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await _technologyRepository.CheckForUniqueName(dto.Name);

            return await _technologyRepository.CreateAsync<TechnologyRequestDto, TechnologyResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if(await _codingChallengeRepository.HasAnyRelatedToTechnologyAsync(id))
            {
                throw new ValidationException("Technology can't be deleted if there are coding challenges related to it.");
            }

            await _technologyRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<TechnologyResponseDto>> GetPageAsync(TechnologiesPagingInfo pagingInfo)
        {
            return await _technologyRepository.GetPageByFilterAsync(pagingInfo);
        }

        public async Task<TechnologyResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _technologyRepository.GetByIdAsync<TechnologyResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, TechnologyRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _validationProvider.TryValidate(dto);

            await _technologyRepository.CheckForUniqueName(dto.Name);

            await _technologyRepository.UpdateAsync(id, dto);
        }
    }
}
