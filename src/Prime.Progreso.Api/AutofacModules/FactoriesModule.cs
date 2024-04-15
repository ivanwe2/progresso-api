using Autofac;
using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Api.AutofacModules
{
    public class FactoriesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(BaseResponseDto).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Factory"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
