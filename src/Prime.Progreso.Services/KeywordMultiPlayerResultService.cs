using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Services
{
    public class KeywordMultiPlayerResultService : IKeywordMultiPlayerResultService
    {
        private readonly IKeywordMultiPlayerResultRepository _keywordMultiPlayerResultRepository;
        private readonly IKeywordRepository _keywordRepository;
        private readonly IValidationProvider _validationProvider;

        public KeywordMultiPlayerResultService(IKeywordMultiPlayerResultRepository keywordMultiPlayerResultRepository,
                                               IKeywordRepository keywordRepository,
                                               IValidationProvider validationProvider)
        {
            _keywordMultiPlayerResultRepository = keywordMultiPlayerResultRepository;
            _keywordRepository = keywordRepository;
            _validationProvider = validationProvider;
        }

        public async Task<KeywordMultiPlayerResultResponseDto> CreateAsync(KeywordMultiPlayerResultRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!await _keywordRepository.HasAnyAsync(dto.KeywordId))
            {
                throw new NotFoundException("Keyword was not found!");
            }

            return await _keywordMultiPlayerResultRepository
                .CreateAsync<KeywordMultiPlayerResultRequestDto, KeywordMultiPlayerResultResponseDto>(dto);
        }
    }
}
