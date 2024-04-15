using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.QuestionCategoryDtos;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Services
{
    public class QuestionCategoryService : IQuestionCategoryService
    {
        private readonly IQuestionCategoryRepository _questionCategoryRepository;
        private IValidationProvider _validationProvider;

        public QuestionCategoryService(IQuestionCategoryRepository questionCategoryRepository, IValidationProvider validationProvider)
        {
            _questionCategoryRepository = questionCategoryRepository;
            _validationProvider = validationProvider;
        }

        public async Task<QuestionCategoryResponseDto> CreateAsync(QuestionCategoryRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            return await _questionCategoryRepository.CreateAsync<QuestionCategoryRequestDto, QuestionCategoryResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _questionCategoryRepository.DeleteAsync(id);
        }

        public async Task<QuestionCategoryResponseDto> GetByIdAsync(Guid id)
        {
            return await _questionCategoryRepository.GetByIdAsync<QuestionCategoryResponseDto>(id);
        }

        public async Task<PaginatedResult<QuestionCategoryResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _questionCategoryRepository.GetPageAsync<QuestionCategoryResponseDto>(pageNumber, pageSize);
        }

        public async Task UpdateAsync(Guid id, QuestionCategoryRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await _questionCategoryRepository.UpdateAsync(id, dto);
        }
    }
}
