using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class KeywordDescriptionSinglePlayerResultRepository 
        : BaseRepository<KeywordDescriptionSinglePlayerResult>, IKeywordDescriptionSinglePlayerResultRepository
    {
        public KeywordDescriptionSinglePlayerResultRepository(ProgresoDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }

        public async Task<KeywordDescriptionSinglePlayerResultResponseDto> CreateAsync(
            KeywordDescriptionSinglePlayerResultWithIsCorrectDto dto)
        {
            var entity = Mapper.Map<KeywordDescriptionSinglePlayerResult>(dto);

            await Items.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return Mapper.Map<KeywordDescriptionSinglePlayerResultResponseDto>(entity);
        }

        public async Task UpdateAsync(Guid id, KeywordDescriptionSinglePlayerResultWithIsCorrectDto dto)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(KeywordDescriptionSinglePlayerResult).Name} was not found!");
            }

            Mapper.Map(dto, entity);

            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }
    }
}
