using Autofac;
using Prime.Progreso.Api.Middleware.Models;
using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Api.AutofacModules
{
    public class HelpersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var apiAssembly = typeof(ProblemDetails).Assembly;
            var domainAssembly = typeof(BaseResponseDto).Assembly;

            builder.RegisterAssemblyTypes(apiAssembly)
                    .Where(t => t.Name.EndsWith("Helper"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(domainAssembly)
                    .Where(t => t.Name.EndsWith("Helper"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
