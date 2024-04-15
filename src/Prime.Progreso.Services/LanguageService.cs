using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.ActivityDtos;
using System.Linq.Expressions;
using Prime.Progreso.Domain.Abstractions.Factories;
using Prime.Progreso.Domain.Dtos.LanguageDtos;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private IValidationProvider _validationProvider;

        public LanguageService(ILanguageRepository languageRepository, IValidationProvider validationProvider)
        {
            _languageRepository = languageRepository;
            _validationProvider = validationProvider;
        }

        public async Task<LanguageResponseDto> CreateAsync(LanguageRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            return await _languageRepository.CreateAsync<LanguageRequestDto, LanguageResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _languageRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<LanguageResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<LanguageResponseDto, bool>> filter = null)
        {
            return await _languageRepository.GetPageAsync(pageNumber, pageSize, filter);
        }

        public async Task<LanguageResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _languageRepository.GetByIdAsync<LanguageResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, LanguageRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _validationProvider.TryValidateAsync(dto);

            await _languageRepository.UpdateAsync(id, dto);
        }
    }
}
