using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Dtos.RandomKeywordDescriptionDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestForms.RandomKeywordDescription;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IKeywordDescriptionService
    {
        Task<KeywordDescriptionResponseDto> CreateAsync(KeywordDescriptionRequestDto dto);
        Task UpdateAsync(Guid id, KeywordDescriptionRequestDto dto);
        Task DeleteAsync(Guid id);
        Task<KeywordDescriptionResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<KeywordDescriptionResponseDto>> GetPageAsync(int pageNumber, int pageSize);
        Task<RandomKeywordDescriptionResponseDto> GetRandomAsync(RandomKeywordDescriptionGetRequestForm randomKeywordDescriptionRequest);
    }
}
