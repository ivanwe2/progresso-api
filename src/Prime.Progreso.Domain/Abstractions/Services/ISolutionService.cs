using Prime.Progreso.Domain.Dtos.SolutionDtos;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface ISolutionService
    {
        Task<SolutionResponseDto> SubmitCodeAsync(SolutionRequestDto dto);
        Task<SolutionResponseDto> GetCodeByCodingChallengeIdAsync(int internId, Guid codingChallengeId);
    }
}
