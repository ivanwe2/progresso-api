using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.TestCaseDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class TestCaseProfile : MapperProfile
    {
        public TestCaseProfile()
        {
            CreateMap<TestCaseRequestDto, TestCase>();
            CreateMap<TestCase, TestCaseResponseDto>();
        }
    }
}
