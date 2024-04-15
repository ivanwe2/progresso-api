using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class KeywordRepository : BaseRepository<Keyword>, IKeywordRepository
    {
        public KeywordRepository(ProgresoDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public override async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
        {
            var entity = await Items
                                .AsNoTracking()
                                .Include(k => k.Language)
                                .Include(k=>k.KeywordDescriptions)
                                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TOutput).Name} was not found!");
            }
            return Mapper.Map<TOutput>(entity);
        }

        public async Task<RandomKeywordResponseDto> GetRandomKeywordAsync(RandomKeywordRequestDto dto)
        {
            var keywords = Items
                         .AsNoTracking()
                         .Where(k => k.Language.Id == dto.LanguageId && k.KeywordDescriptions.Any(kd => dto.Difficulties.Contains(kd.Difficulty)))
                         .Select(i => new RandomKeywordResponseDto() { KeywordId = i.Id, Word = i.Word });

            var keywordsCount = await keywords.CountAsync();

            if (keywordsCount == 0)
            {
                throw new NotFoundException("There is no keyword with this language and/or difficulty level!");
            }

            var random = new Random();
            var index = random.Next(keywordsCount);

            var randomKeyword = await keywords.Skip(index).FirstAsync();

           return randomKeyword;
        }

        public async Task<bool> DoAllKeywordsExist(List<Guid> keywordIds)
        {
            var count = await Items.Where(e => keywordIds.Contains(e.Id)).CountAsync();

            return count == keywordIds.Count;
        }
    }
}
