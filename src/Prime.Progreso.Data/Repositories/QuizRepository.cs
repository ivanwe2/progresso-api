using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.Pagination.Quiz;

namespace Prime.Progreso.Data.Repositories
{
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<QuizResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await Items.Include(x => x.QuizQuestionLinks)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id);

            if(entity is null)
            {
                throw new NotFoundException("Invalid quiz ID.");
            }

            return Mapper.Map<QuizResponseDto>(entity);
        }

        public async Task UpdateAsync(Guid id, QuizRequestDto dto)
        {
            var entity = await Items.Include(x => x.QuizQuestionLinks)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Invalid quiz ID.");
            }

            Mapper.Map(dto, entity);

            DbContext.Entry(entity).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();
        }

        public async Task<PaginatedResult<QuizResponseDto>> GetPageByFilterAsync(QuizesPagingInfo pagingInfo)
        {
            var query = Items.AsQueryable();

            if (pagingInfo.quizIds is not null)
            {
                query = query.Where(q => pagingInfo.quizIds.Contains(q.Id));
            }

            return await Mapper.ProjectTo<QuizResponseDto>(query).PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task<bool> IsQuestionRelatedToQuizAsync(Guid id, Guid questionId)
            => await Items.AnyAsync(q=> q.Id == id && q.Questions.Any(s => s.Id == questionId));
    }
}
