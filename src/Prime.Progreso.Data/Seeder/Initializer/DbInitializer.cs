using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data.Seeder.Keywords;
using Prime.Progreso.Domain.Abstractions.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Seeder.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ProgresoDbContext _context;
        private readonly IKeywordsSeeder _keywordsSeeder;
        public DbInitializer(ProgresoDbContext context, IKeywordsSeeder keywordsSeeder)
        {
            _context = context;
            _keywordsSeeder = keywordsSeeder;
        }
        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();
            if (!await _context.Keywords.AnyAsync())
            {
                await _keywordsSeeder.SeedAsync();
            }
        }
    }
}
