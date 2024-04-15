using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Repositories
{
    public class QuizAssignmentRepository : BaseRepository<QuizAssignment>, IQuizAssignmentRepository
    {
        public QuizAssignmentRepository(ProgresoDbContext context, IMapper mapper) 
            : base(context, mapper)
        {
        }

        public async Task<TOutput> GetByIdAsync<TOutput>(Guid id, int userId)
        {
            var outputDto = await Mapper.ProjectTo<TOutput>(Items.AsNoTracking().Where(c => c.AssigneeId == userId && c.Id == id))
                .FirstOrDefaultAsync();

            if (outputDto is null)
            {
                throw new NotFoundException($"Quiz Assignment was not found!");
            }

            return outputDto;
        }

        public async Task<PaginatedResult<QuizAssignmentResponseDto>> GetPageByFilterAsync(PagingInfo pagingInfo, int userId)
        {
            var query = Items.AsQueryable();

            query = query.Where(m => m.AssigneeId == userId);

            return await Mapper.ProjectTo<QuizAssignmentResponseDto>(query).PaginateAsync(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task<List<Guid>> GetQuizIdsAssignedToUser(PagingInfo pagingInfo, int userId, DateTime timeOfRequestedAccess)
        {
            var query = Items.AsQueryable();

            var pagedResult = await query
                .Where(q => q.AssigneeId == userId && q.StartTime <= timeOfRequestedAccess && q.EndTime >= timeOfRequestedAccess)
                .Select(q => q.QuizId)
                .PaginateAsync(pagingInfo.Page, pagingInfo.Size);

            return pagedResult.Content;
        }

        public async Task<bool> IsInternAssignedToQuizAsync(Guid quizId, int userId)
        {
            return await Items.AnyAsync(q=> q.QuizId == quizId && q.AssigneeId == userId 
                                            && q.StartTime <= DateTime.UtcNow && q.EndTime >= DateTime.UtcNow);
        }
    }
}
