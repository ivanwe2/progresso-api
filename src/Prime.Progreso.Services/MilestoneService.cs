using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos.Milestones;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Validators;
using IValidatorFactory = Prime.Progreso.Domain.Abstractions.Factories.IValidatorFactory;

namespace Prime.Progreso.Services
{
    public class MilestoneService : IMilestoneService
    {
        private readonly IMilestoneRepository _milestoneRepository;
        private IValidationProvider _validationProvider;

        public MilestoneService(IMilestoneRepository milestoneRepository, IValidationProvider validationProvider)
        {
            _milestoneRepository = milestoneRepository;
            _validationProvider = validationProvider;
        }

        public async Task<MilestoneResponseDto> CreateAsync(MilestoneRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            MilestoneResponseDto createdMilestone = await _milestoneRepository.CreateAsync<MilestoneRequestDto, MilestoneResponseDto>(dto);

            return createdMilestone;
        }

        public async Task UpdateAsync(Guid id, MilestoneRequestDto dto)
        {
            await _validationProvider.TryValidateAsync(dto);

            await _milestoneRepository.UpdateAsync<MilestoneRequestDto>(id, dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _milestoneRepository.DeleteAsync(id);
        }

        public async Task<MilestoneResponseDto> GetByIdAsync(Guid id)
        {
            MilestoneResponseDto milestone = await _milestoneRepository.GetByIdAsync<MilestoneResponseDto>(id);

            return milestone;
        }

        public async Task<PaginatedResult<MilestoneResponseDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _milestoneRepository.GetPageAsync<MilestoneResponseDto>(pageNumber, pageSize);
        }
    }
}
