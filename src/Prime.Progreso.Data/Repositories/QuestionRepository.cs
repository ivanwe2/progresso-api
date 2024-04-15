using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.AnswerDtos;
using Prime.Progreso.Domain.Dtos.QuestionDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<QuestionResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await Items.Include(c => c.CategorizedQuestions)
                                    .Include(a => a.Answers)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(c => c.Id == id);

            if(entity is null)
            {
                throw new NotFoundException("Invalid question ID.");
            }

            return Mapper.Map<QuestionResponseDto>(entity);
        }

        public async Task UpdateAsync(Guid id, QuestionRequestDto dto)
        {
            var entity = await Items.Include(q => q.CategorizedQuestions)
                                    .Include(a => a.Answers)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            if(entity is null)
            {
                throw new NotFoundException("Invalid question ID.");
            }

            Mapper.Map(dto, entity);

            DbContext.Entry(entity).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();
        }

        public async Task<List<Guid>> GetQuestionIdsByQuizIdAsync(Guid quizId)
        {
            return await DbContext.QuizQuestionLinks
                .Where(x => x.QuizId == quizId)
                .Select(x => x.QuestionId)
                .ToListAsync();
        }

        public async Task<List<QuestionStatisticsResponseDto>> CalculateSuccessRateForQuestionStatisticsAsync(
            List<QuestionStatisticsResponseDto> questions)
        {
            foreach (var question in questions)
            {
                var entity = await Items
                    .Include(x => x.Answers)
                    .FirstOrDefaultAsync(x => x.Id == question.Id);

                question.Title = entity.Title;

                foreach (var answer in entity.Answers)
                {
                    if (!question.Answers.Any(x => x.Id == answer.Id))
                    {
                        question.Answers.Add(new AnswerStatisticsResponseDto
                        {
                            Id = answer.Id,
                            Content = answer.Content,
                            ChoiceRate = 0
                        });
                    }

                    if (answer.IsCorrect)
                    {
                        question.CompletionRate = question.Answers
                            .FirstOrDefault(a => a.Id == answer.Id).ChoiceRate;
                    }                        
                }               
            }

            return questions;
        }

        public async Task<bool> DoAllQuestionsExist(List<Guid> questionIds)
        {
            var count = await Items.Where(e => questionIds.Contains(e.Id)).CountAsync();

            return count == questionIds.Count;
        }
    }
}
