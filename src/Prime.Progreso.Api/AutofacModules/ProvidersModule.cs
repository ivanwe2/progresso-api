using Autofac;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Api.AutofacModules
{
    public class ProvidersModiule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(BaseResponseDto).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Provider"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
