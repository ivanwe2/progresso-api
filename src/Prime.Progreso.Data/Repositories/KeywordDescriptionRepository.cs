using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.RandomKeywordDescriptionDtos;
using Prime.Progreso.Domain.Enums;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class KeywordDescriptionRepository : BaseRepository<KeywordDescription>, IKeywordDescriptionRepository
    {
        public KeywordDescriptionRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public override async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
        {
            var entity = await Items
                                .AsNoTracking()
                                .Include(k => k.Keyword)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TOutput).Name} was not found!");
            }

            return await Mapper.ProjectTo<TOutput>(Items.AsQueryable().AsNoTracking()
                .Where(c => c.Id == entity.Id)).FirstOrDefaultAsync();
        }

        public async Task<RandomKeywordDescriptionResponseDto> GetRandomAsync(Guid languageId, List<Difficulty> difficultyLevels)
        {
            var query = Items
                .AsQueryable()
                .AsNoTracking();

            query = query.Where(x => x.Keyword.LanguageId == languageId);

            if (difficultyLevels != null && difficultyLevels.Any())
            {
                query = query.Where(x => difficultyLevels.Contains(x.Difficulty));
            }

            int count = await query.CountAsync();

            if (count == 0)
            {
                return null;
            }

            var random = new Random();
            var index = random.Next(count);

            var keywordDescription = await query.Skip(index).FirstAsync();

            return Mapper.Map<RandomKeywordDescriptionResponseDto>(keywordDescription);
        }

        public async Task<bool> CheckIfAnswerIsCorrect(Guid id, string answer)
        {
            var result = await Items
                .AsNoTracking()
                .Include(k => k.Keyword)
                .FirstOrDefaultAsync(x => x.Id == id && x.Keyword.Word.ToLower() == answer.ToLower());

            if (result is null)
                return false;

            return true;
        }
    }
}