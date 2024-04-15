using Microsoft.EntityFrameworkCore;
using Prime.Progreso.Data;
using Prime.Progreso.Data.Seeder.Initializer;
using Prime.Progreso.Domain.Abstractions.Seeders;

namespace Prime.Progreso.Api.Extenions
{
    public static class AppBuilderExtensions
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ProgresoDbContext>();
            context.Database.Migrate();
        }

        public static async Task SeedKeywordsDataAsync(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var seeder = serviceScope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await seeder.SeedAsync();
        }
    }
}
