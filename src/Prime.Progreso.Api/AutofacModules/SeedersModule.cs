using Autofac;
using Prime.Progreso.Data.Entities;

namespace Prime.Progreso.Api.AutofacModules
{
    public class SeedersModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(BaseEntity).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Seeder"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
