using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Dtos.RandomKeywordDescriptionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestForms.RandomKeywordDescription;

namespace Prime.Progreso.Services
{
    public class KeywordDescriptionService : IKeywordDescriptionService
    {
        private readonly IKeywordDescriptionRepository _keywordDescriptionRepository;
        private readonly IKeywordRepository _keywordRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidationProvider _validationProvider;

        public KeywordDescriptionService(IKeywordDescriptionRepository keywordDescriptionRepository,
                                         IKeywordRepository keywordRepository,
                                         ILanguageRepository languageRepository,
                                         IValidationProvider validationProvider)
        {
            _keywordDescriptionRepository = keywordDescriptionRepository;
            _keywordRepository = keywordRepository;
            _languageRepository = languageRepository;
            _validationProvider = validationProvider;
        }

        public async Task<KeywordDescriptionResponseDto> CreateAsync(KeywordDescriptionRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await DoesKeywordExistAsync(dto.KeywordId);

            KeywordDescriptionResponseDto createdKeywordDescription = await _keywordDescriptionRepository.CreateAsync<KeywordDescriptionRequestDto, KeywordDescriptionResponseDto>(dto);

            return createdKeywordDescription;
        }

        public async Task UpdateAsync(Guid id, KeywordDescriptionRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await DoesKeywordExistAsync(dto.KeywordId);

            await _keywordDescriptionRepository.UpdateAsync(id, dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _keywordDescriptionRepository.DeleteAsync(id);
        }

        public async Task<KeywordDescriptionResponseDto> GetByIdAsync(Guid id)
        {
            KeywordDescriptionResponseDto keywordDescription = await _keywordDescriptionRepository.GetByIdAsync<KeywordDescriptionResponseDto>(id);

            return keywordDescription;
        }

        public async Task<PaginatedResult<KeywordDescriptionResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _keywordDescriptionRepository.GetPageAsync<KeywordDescriptionResponseDto>(pageNumber, pageSize);
        }

        public async Task<RandomKeywordDescriptionResponseDto> GetRandomAsync(
            RandomKeywordDescriptionGetRequestForm randomKeywordDescriptionRequest)
        {
            await DoesLanguageExistAsync(randomKeywordDescriptionRequest.LanguageId);

            return await _keywordDescriptionRepository.GetRandomAsync(
                randomKeywordDescriptionRequest.LanguageId, 
                randomKeywordDescriptionRequest.DifficultyLevels);
        }

        private async Task DoesKeywordExistAsync(Guid id)
        {
            if (!await _keywordRepository.HasAnyAsync(id))
                throw new NotFoundException("Keyword was not found!");
        }

        private async Task DoesLanguageExistAsync(Guid languageId)
        {
            var languageExists = await _languageRepository.HasAnyAsync(languageId);

            if(!languageExists)
            {
                throw new NotFoundException("Language was not found!");
            }
        }
    }
}
