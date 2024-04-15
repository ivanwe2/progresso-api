using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Abstractions.Seeders
{
    public interface IKeywordsSeeder
    {
        Task SeedAsync();
    }
}
