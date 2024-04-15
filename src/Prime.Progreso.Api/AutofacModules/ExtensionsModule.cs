using Autofac;
using Prime.Progreso.Api.Middleware.Models;

namespace Prime.Progreso.Api.AutofacModules
{
    public class ExtensionsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ProblemDetails).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Extensions"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
