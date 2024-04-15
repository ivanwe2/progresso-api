using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;
using Prime.Progreso.Domain.Dtos.SolutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Data.Repositories
{
    public class AssignmentToChallengeRepository : BaseRepository<AssignmentToCodingChallenge>, IAssignmentToChallengeRepository
    {
        public AssignmentToChallengeRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<AssignmentResponseDto> AddOrUpdateAsync(AssignmentRequestDto dto)
        {
            var entity = await Items.FirstOrDefaultAsync(a => a.InternId == dto.InternId
                                                             && a.CodingChallengeId == dto.CodingChallengeId);
            if (entity is null)
            {
                entity = Mapper.Map<AssignmentToCodingChallenge>(dto);

                var codingChallenge = await DbContext.CodingChallenges.FirstOrDefaultAsync(x => x.Id == entity.CodingChallengeId);
                entity.SolutionPath = codingChallenge.Codebase;

                await Items.AddAsync(entity);
            }
            else
            {
                Mapper.Map(dto, entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            await DbContext.SaveChangesAsync();

            return await Mapper.ProjectTo<AssignmentResponseDto>(Items.AsQueryable().AsNoTracking()
                .Where(c => c.Id == entity.Id)).FirstOrDefaultAsync();
        }

        public async Task<List<Guid>> GetAssignedCodingChallengeIdsAsync(int userId)
        {
            var currentDateTimeUtc = DateTime.UtcNow;

            var codingChallengeIds = await Items.Where(a => a.InternId == userId && currentDateTimeUtc >= a.StartTime && 
                                                            (a.EndTime == null || currentDateTimeUtc <= a.EndTime))
                                                .Select(a => a.CodingChallengeId)
                                                .ToListAsync();

            return codingChallengeIds;
        }

        public async Task<AssignmentResponseDto> GetByIdAsync(Guid id, int userId)
        {
            var entity = await Items.AsNoTracking()
                                    .FirstOrDefaultAsync(a => a.Id == id && a.InternId == userId);
            if (entity is null)
            {
                throw new NotFoundException($"Assignment was not found!");
            }

            return Mapper.Map<AssignmentResponseDto>(entity);
        }

        public async Task<AssignmentResponseDto> GetByInternAndChallengeIdsAsync(int internId, Guid codingChallengeId)
        {
            var entity = await Items.AsNoTracking()
                                    .FirstOrDefaultAsync(a => a.InternId == internId && a.CodingChallengeId == codingChallengeId);
            if (entity is null)
            {
                throw new NotFoundException($"Assignment was not found!");
            }

            return Mapper.Map<AssignmentResponseDto>(entity);
        }


        public async Task<PaginatedResult<AssignmentResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber,
                                                                                                int pageSize,
                                                                                                int userId)
        {
            var query = Items.AsQueryable();

            query = query.Where(m => m.InternId == userId);

            return await Mapper.ProjectTo<AssignmentResponseDto>(query).PaginateAsync(pageNumber, pageSize);
        }

        public async Task<bool> HasAnyAsync(Guid codingChallengeId, int userId)
        {
            return await Items.AnyAsync(a => a.CodingChallengeId == codingChallengeId && a.InternId == userId);
        }

        public async Task UnassignInternAsync(UnassignmentRequestDto dto)
        {
            var entity = await Items.FirstOrDefaultAsync(a => a.InternId == dto.InternId
                                                             && a.CodingChallengeId == dto.CodingChallengeId);
            if (entity is null)
            {
                throw new NotFoundException($"Assignment was not found!");
            }

            Mapper.Map(dto, entity);

            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task<SolutionResponseDto> UpdateSolutionPathAsync(int userId, SolutionRequestDto dto)
        {
            var entity = await Items.FirstOrDefaultAsync(a => a.InternId == userId
                                                             && a.CodingChallengeId == dto.CodingChallengeId);

            if (entity is null)
            {
                throw new NotFoundException($"Assignment was not found!");
            }

            Mapper.Map(dto, entity);
            DbContext.Entry(entity).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return Mapper.Map<SolutionResponseDto>(entity);
        }

        public async Task<SolutionResponseDto> GetSolutionByInternAndChallengeIdsAsync(int internId, Guid codingChallengeId)
        {
            var entity = await Items.AsNoTracking()
                                    .FirstOrDefaultAsync(a => a.InternId == internId && a.CodingChallengeId == codingChallengeId);
            if (entity is null)
            {
                throw new NotFoundException($"Assignment was not found!");
            }

            return new SolutionResponseDto { AssignmentId = entity.Id, SolutionCode = entity.SolutionPath };
        }
    }
}
