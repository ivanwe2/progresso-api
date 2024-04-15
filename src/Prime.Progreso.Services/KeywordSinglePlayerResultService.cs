using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.KeywordSinglePlayerResult;

namespace Prime.Progreso.Services
{
    public class KeywordSinglePlayerResultService : IKeywordSinglePlayerResultService
    {
        private readonly IKeywordSinglePlayerResultRepository _keywordSinglePlayerResultRepository;
        private readonly IKeywordRepository _keywordRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidationProvider _validationProvider;

        public KeywordSinglePlayerResultService(IKeywordSinglePlayerResultRepository keywordSinglePlayerResultRepository,
                                                IKeywordRepository keywordRepository,
                                                ILanguageRepository languageRepository,
                                                IValidationProvider validationProvider)
        {
            _keywordSinglePlayerResultRepository = keywordSinglePlayerResultRepository;
            _keywordRepository = keywordRepository;
            _languageRepository = languageRepository;
            _validationProvider = validationProvider;
        }

        public async Task<KeywordSinglePlayerResultResponseDto> CreateAsync(KeywordSinglePlayerResultRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!await _keywordRepository.HasAnyAsync(dto.KeywordId))
            {
                throw new NotFoundException("Keyword was not found!");
            }

            return await _keywordSinglePlayerResultRepository.CreateAsync<KeywordSinglePlayerResultRequestDto, KeywordSinglePlayerResultResponseDto>(dto);
        }

        public async Task UpdateIsCorrectAsync(Guid id, KeywordSinglePlayerResultIsCorrectUpdateRequestDto dto)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _validationProvider.TryValidateAsync(dto);

            await _keywordSinglePlayerResultRepository.UpdateAsync(id, dto);
        }
        
        public async Task<PaginatedResult<KeywordSinglePlayerResultResponseDto>> GetPageAsync(
            KeywordSinglePlayerResultPagingInfo pagingInfo)
        {
            var keywordsExist = await _keywordRepository.DoAllKeywordsExist(pagingInfo.KeywordIds);

            if (!keywordsExist)
                throw new NotFoundException("Keyword was not found!");

            var languagesExist = await _languageRepository.DoAllLanguagesExist(pagingInfo.LanguageIds);

            if (!languagesExist)
                throw new NotFoundException("Language was not found!");

            return await _keywordSinglePlayerResultRepository.GetPageAsync(pagingInfo);
        }
    }
}
