using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Services
{
    public class KeywordDescriptionMultiPlayerResultService : IKeywordDescriptionMultiPlayerResultService
    {
        private readonly IKeywordDescriptionMultiPlayerResultRepository _keywordDescriptionMultiPlayerResultRepository;
        private readonly IKeywordDescriptionRepository _keywordDescriptionRepository;
        private readonly IValidationProvider _validationProvider;

        public KeywordDescriptionMultiPlayerResultService(
            IKeywordDescriptionMultiPlayerResultRepository keywordDescriptionMultiPlayerResultRepository,
            IValidationProvider validationProvider,
            IKeywordDescriptionRepository keywordDescriptionRepository)
        {
            _keywordDescriptionMultiPlayerResultRepository = keywordDescriptionMultiPlayerResultRepository;
            _validationProvider = validationProvider;
            _keywordDescriptionRepository = keywordDescriptionRepository;
        }

        public async Task<KeywordDescriptionMultiPlayerResultResponseDto> CreateAsync(
            KeywordDescriptionMultiPlayerResultRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            if (!await _keywordDescriptionRepository.HasAnyAsync(dto.KeywordDescriptionId))
            {
                throw new NotFoundException("Invalid keyword description ID.");
            }

            return await _keywordDescriptionMultiPlayerResultRepository
                .CreateAsync<KeywordDescriptionMultiPlayerResultRequestDto, KeywordDescriptionMultiPlayerResultResponseDto>(dto);
        }
    }
}
