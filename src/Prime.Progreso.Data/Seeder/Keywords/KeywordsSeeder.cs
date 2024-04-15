using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Abstractions.Seeders;
using Prime.Progreso.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Prime.Progreso.Data.Seeder.Keywords
{
    public class KeywordsSeeder : IKeywordsSeeder
    {
        private ProgresoDbContext Context { get; }

        public KeywordsSeeder(ProgresoDbContext context)
        {
            Context = context;
        }

        private async Task<Dictionary<string,string>> GetDataAsync(string fileName)
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            string folderPath = Path.Combine("Seeder", "Keywords", "Data");
            string filePath = Path.GetFullPath(Path.Combine(rootPath, folderPath, fileName + ".txt"));

            var dictionary = new Dictionary<string,string>();

            using (var sr = new StreamReader(filePath))
            {
                string line = await sr.ReadLineAsync();

                while (line != null)
                {
                    if (line != string.Empty)
                    {
                        List<string> keyword = line.Split(":", 2 ,StringSplitOptions.TrimEntries).ToList();
                        for (int i = 1; i < keyword.Count; i++) 
                        {
                            if (keyword[i] != string.Empty)
                                dictionary.Add(keyword[0], keyword[i]);
                        }
                    }
                    line = await sr.ReadLineAsync()!;
                }
            }
            return dictionary;
        }
        private async Task SeedByLanguageAsync(string languageName)
        {
            var data = await GetDataAsync(languageName + "-keywords");

            var language = await Context.Set<Language>().FirstOrDefaultAsync(l => l.Name == languageName);
            if (language == null)
            {
                language = new Language() { Name = languageName };
                await Context.Set<Language>().AddAsync(language);
            }

            foreach (var item in data)
            {
                var keyword = new Keyword() { LanguageId = language.Id, Word = item.Key };
                await Context.Set<Keyword>().AddAsync(keyword);

                List<string> descriptions = item.Value.Split(":", StringSplitOptions.TrimEntries).ToList();
                for (int i = 0; i < descriptions.Count; i++)
                {
                    var keywordDescription = new KeywordDescription() { Description = descriptions[i], KeywordId = keyword.Id, Difficulty = Difficulty.Easy };
                    await Context.Set<KeywordDescription>().AddAsync(keywordDescription);
                }
                
            }
            await Context.SaveChangesAsync();
        }

        public async Task SeedAsync()
        {
            await SeedByLanguageAsync("C#");
            await SeedByLanguageAsync("Dart");
            await SeedByLanguageAsync("Java");
            await SeedByLanguageAsync("JavaScript");
        }
    }
}
