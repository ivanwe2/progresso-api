using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionCategoryRepository _questionCategoryRepository;
        private readonly IValidationProvider _validationProvider;

        public QuestionService(IQuestionRepository questionRepository,
                               IQuestionCategoryRepository questionCategoryRepository,
                               IValidationProvider validationProvider) 
        {
            _questionRepository = questionRepository;
            _questionCategoryRepository = questionCategoryRepository;
            _validationProvider = validationProvider;
        }

        public async Task<QuestionResponseDto> CreateAsync(QuestionRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await ValidateQuestionCategoriesAsync(dto);

            return await _questionRepository.CreateAsync<QuestionRequestDto, QuestionResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _questionRepository.DeleteAsync(id);
        }

        public async Task<QuestionResponseDto> GetByIdAsync(Guid id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        public async Task<PaginatedResult<QuestionResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _questionRepository.GetPageAsync<QuestionResponseDto>(pageNumber, pageSize);
        }

        public async Task UpdateAsync(Guid id, QuestionRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await ValidateQuestionCategoriesAsync(dto);

            await _questionRepository.UpdateAsync(id, dto);
        }

        private async Task ValidateQuestionCategoriesAsync(QuestionRequestDto dto)
        {
            var isValid = await _questionCategoryRepository.DoAllCategoriesExistAsync(dto.QuestionCategoryIds);

            if (!isValid)
            {
                throw new NotFoundException("One or more question category IDs are invalid.");
            }
        }

    }
}
