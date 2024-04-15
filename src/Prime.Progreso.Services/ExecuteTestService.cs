using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;

namespace Prime.Progreso.Services
{
    public class ExecuteTestService : IExecuteTestService
    {
        private readonly ITechnologyRepository _technologyRepository;

        public ExecuteTestService(ITechnologyRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        public async Task<bool> IsTestCaseExecutionSuccessful(Guid technologyId, TestCaseRequestDto dto)
        {
            var technology = await _technologyRepository.GetByIdAsync<TechnologyResponseDto>(technologyId);

            string expectedOutput = dto.ExpectedOutput;
            string actualOutput = await ExecuteCode(technology, dto.InputData);

            if (string.Equals(actualOutput, expectedOutput))
            {
                return true;
            }

            return false;
        }

        private async Task<string> ExecuteCode(TechnologyResponseDto technology, string input)
        {
            string errors = "";
            string output = "";

            // Code to execute the test case for the specific technology

            if (!string.IsNullOrEmpty(errors.Trim()))
            {
                Console.WriteLine(errors);
            }

            return output;
        }
    }
}
