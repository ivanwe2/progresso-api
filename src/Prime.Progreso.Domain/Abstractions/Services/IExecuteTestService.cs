using Prime.Progreso.Domain.Dtos.TestCaseDtos;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface IExecuteTestService
    {
        Task<bool> IsTestCaseExecutionSuccessful(Guid technologyId, TestCaseRequestDto dto);
    }
}
