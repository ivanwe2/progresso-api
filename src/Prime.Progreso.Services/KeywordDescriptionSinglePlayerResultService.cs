using AutoMapper;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System.Linq.Expressions;

namespace Prime.Progreso.Services
{
    public class KeywordDescriptionSinglePlayerResultService : IKeywordDescriptionSinglePlayerResultService
    {
        private readonly IKeywordDescriptionSinglePlayerResultRepository _keywordDescriptionSinglePlayerResultRepository;
        private readonly IKeywordDescriptionRepository _keywordDescriptionRepository;
        private readonly IValidationProvider _validationProvider;
        private readonly IMapper _mapper;

        public KeywordDescriptionSinglePlayerResultService(
            IKeywordDescriptionSinglePlayerResultRepository keywordDescriptionSinglePlayerResultRepository,
            IValidationProvider validationProvider,
            IKeywordDescriptionRepository keywordDescriptionRepository,
            IMapper mapper)
        {
            _keywordDescriptionSinglePlayerResultRepository = keywordDescriptionSinglePlayerResultRepository;
            _validationProvider = validationProvider;
            _keywordDescriptionRepository = keywordDescriptionRepository;
            _mapper = mapper;
        }

        public async Task<KeywordDescriptionSinglePlayerResultResponseDto> CreateAsync(
            KeywordDescriptionSinglePlayerResultRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await CheckIfKeywordDescriptionExists(dto.KeywordDescriptionId);

            var isAnswerCorrect = await _keywordDescriptionRepository
                .CheckIfAnswerIsCorrect(dto.KeywordDescriptionId, dto.Answer);

            var createDto = _mapper.Map<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>(dto);
            createDto.IsCorrect = isAnswerCorrect;

            return await _keywordDescriptionSinglePlayerResultRepository.CreateAsync(createDto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _keywordDescriptionSinglePlayerResultRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<KeywordDescriptionSinglePlayerResultResponseDto>> GetPageAsync(
            int pageNumber, int pageSize, Expression<Func<KeywordDescriptionSinglePlayerResultResponseDto, bool>> filter = null)
        {
            return await _keywordDescriptionSinglePlayerResultRepository.GetPageAsync(pageNumber, pageSize, filter);
        }

        public async Task<KeywordDescriptionSinglePlayerResultResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _keywordDescriptionSinglePlayerResultRepository
                .GetByIdAsync<KeywordDescriptionSinglePlayerResultResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, KeywordDescriptionSinglePlayerResultRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _validationProvider.TryValidate(dto);

            await CheckIfKeywordDescriptionExists(dto.KeywordDescriptionId);

            var isAnswerCorrect = await _keywordDescriptionRepository
                .CheckIfAnswerIsCorrect(dto.KeywordDescriptionId, dto.Answer);

            var updateDto = _mapper.Map<KeywordDescriptionSinglePlayerResultWithIsCorrectDto>(dto);
            updateDto.IsCorrect = isAnswerCorrect;

            await _keywordDescriptionSinglePlayerResultRepository.UpdateAsync(id, updateDto);
        }

        private async Task CheckIfKeywordDescriptionExists(Guid keywordDescriptionId)
        {
            var keywordDescriptionExists = await _keywordDescriptionRepository.HasAnyAsync(keywordDescriptionId);

            if (!keywordDescriptionExists)
            {
                throw new NotFoundException("Invalid keyword description ID.");
            }
        }
    }
}
