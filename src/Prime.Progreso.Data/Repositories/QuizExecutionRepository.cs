using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Pagination.Quiz;

namespace Prime.Progreso.Data.Repositories
{
    public class QuizExecutionRepository : BaseRepository<QuizExecution>, IQuizExecutionRepository
    {
        public QuizExecutionRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<QuizExecutionResponseDto> GetByIdAsync(Guid id, int userId)
        {
            var entity = await Items
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (entity is null)
            {
                throw new NotFoundException($"Quiz execution was not found!");
            }

            return Mapper.Map<QuizExecutionResponseDto>(entity);
        }

        public async Task<PaginatedResult<QuizExecutionResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            var query = Items.AsQueryable();

            query = query.Where(m => m.UserId == userId);

            return await Mapper.ProjectTo<QuizExecutionResponseDto>(query).PaginateAsync(pageNumber, pageSize);
        }

        public async Task<List<QuizStatisticsResponseDto>> GetAllQuizStatisticsAsync(QuizStatisticsPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            var quizExecutions = DbContext.QuizExecutions.Include(x => x.Quiz).AsQueryable();

            if (pagingInfo.userIds is not null && pagingInfo.userIds.Any())
            {
                quizExecutions = quizExecutions.Where(q => pagingInfo.userIds.Contains(q.UserId));
            }

            return await quizExecutions.Select(x => new QuizStatisticsResponseDto
            {
                Id = x.QuizId,
                Title = x.Quiz.Title
            }).ToListAsync();
        }

        public async Task<bool> IsRelatedToUserAsync(Guid id, int userId)
            => await Items.AnyAsync(c => c.Id == id && c.UserId == userId);
    }
}
