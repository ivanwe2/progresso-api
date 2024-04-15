using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Data.Extensions;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;
using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Data.Repositories
{
    public class AnswerChoiceRepository : BaseRepository<AnswerChoice>, IAnswerChoiceRepository
    {
        public AnswerChoiceRepository(ProgresoDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<AnswerChoiceResponseDto> GetByIdAsync(Guid id, int userId)
        {
            var entity = await Items
                .Include(c => c.QuizExecution)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id && c.QuizExecution.UserId == userId);

            if (entity is null)
            {
                throw new NotFoundException($"Answer choice was not found!");
            }

            return Mapper.Map<AnswerChoiceResponseDto>(entity);
        }

        public async Task<PaginatedResult<AnswerChoiceResponseDto>> GetPageAndFilterByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            var query = Items.AsQueryable();

            query = query.Where(m => m.QuizExecution.UserId == userId);

            return await Mapper.ProjectTo<AnswerChoiceResponseDto>(query).PaginateAsync(pageNumber, pageSize);
        }

        private async Task<List<AnswerStatisticsResponseDto>> GetStatisticsByQuizAndQuestionIdAsync(Guid quizId, Guid questionId)
        {
            var answerChoices = await Items
                .Include(c => c.QuizExecution)
                .Where(c => c.QuizExecution.QuizId == quizId && c.QuestionId == questionId)
                .ToListAsync();

            var result = new List<AnswerStatisticsResponseDto>();

            double choiceRate;
            int count = answerChoices.Count;

            if (count > 0)
            {
                foreach (var choice in answerChoices)
                {
                    if (!result.Any(x => x.Id == choice.Id))
                    {                        
                        int choiceCount = answerChoices.Count(x => x.ChoiceId == choice.ChoiceId);
                        choiceRate = Math.Round((double) choiceCount / (double) count, 2);
                        var answerChoice = await DbContext.Answers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == choice.ChoiceId);
                        result.Add(new AnswerStatisticsResponseDto
                        {
                            Id = answerChoice.Id,
                            Content = answerChoice.Content,
                            ChoiceRate = choiceRate
                        });
                    }                   
                }
            }           

            return result;
        }

        public async Task<List<QuestionStatisticsResponseDto>> GetStatisticsByQuizAndQuestionIdsAsync(Guid quizId, List<Guid> questionIds)
        {
            var result = new List<QuestionStatisticsResponseDto>();

            foreach (var questionId in questionIds)
            {
                var answerChoices = await GetStatisticsByQuizAndQuestionIdAsync(quizId, questionId);
                result.Add(new QuestionStatisticsResponseDto
                {
                    Id = questionId,
                    Answers = answerChoices
                });
            }

            return result;
        }
    }
}
