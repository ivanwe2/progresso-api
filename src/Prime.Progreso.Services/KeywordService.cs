using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services
{
    public class KeywordService : IKeywordService
    {
        private readonly IKeywordRepository _repository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidationProvider _validationProvider;

        public KeywordService(IKeywordRepository keywordRepository,ILanguageRepository languageRepository, IValidationProvider validationProvider)
        {
            _repository = keywordRepository;
            _languageRepository = languageRepository;
            _validationProvider = validationProvider;
        }

        public async Task<KeywordResponseDto> CreateAsync(KeywordRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await DoesLanguageExist(dto.LanguageId);

            return await _repository.CreateAsync<KeywordRequestDto,KeywordResponseDto>(dto);
        }

        public async Task UpdateAsync(Guid id, KeywordRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await DoesLanguageExist(dto.LanguageId);

            await _repository.UpdateAsync(id, dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<KeywordResponseDto> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync<KeywordResponseDto>(id);
        }

        public async Task<PaginatedResult<KeywordResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetPageAsync<KeywordResponseDto>(pageNumber, pageSize);
        }

        public async Task<RandomKeywordResponseDto> GetRandomKeywordAsync(RandomKeywordRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            if (!await _languageRepository.HasAnyAsync(dto.LanguageId))
            {
                throw new NotFoundException("Language was not found!");
            }

            return await _repository.GetRandomKeywordAsync(dto);
        }

        public async Task DoesLanguageExist(Guid id)
        {
            if (!await _languageRepository.HasAnyAsync(id))
                throw new NotFoundException("Language was not found!");
        }
    }
}
