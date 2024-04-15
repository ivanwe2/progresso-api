using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Prime.Progreso.Api.AutofacModules;
using Prime.Progreso.Api.Extenions;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.PathDtos;
using Prime.Progreso.Domain.Helpers;
using Prime.Progreso.Domain.Notifications.Hubs;
using Serilog;
using Serilog.Events;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
    {
        autofacBuilder.RegisterModule<ServicesModule>();
        autofacBuilder.RegisterModule<RepositoriesModule>();
        autofacBuilder.RegisterModule<FactoriesModule>();
        autofacBuilder.RegisterModule<ExtensionsModule>();
        autofacBuilder.RegisterModule<ProvidersModiule>();
        autofacBuilder.RegisterModule<HelpersModule>();
        autofacBuilder.RegisterModule<SeedersModule>();
    })
    .UseSerilog((context, services, configuration) =>
        configuration
          .ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Prime.Progreso.Domain"));

builder.Services.AddControllers();
builder.Services.ConfigureCustomModelStateResponseFactory();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options => builder.Configuration.Bind("JwtSettings", options));

builder.Services.AddEndpointsApiExplorer()
                .AddProgresoSwagger()
                .AddProgresoAutomapper()
                .AddProgresoDbContext(builder.Configuration.GetConnectionString("Default"))
                .AddDbInitializer()
                .AddCors()
                .AddLogging()
                .AddNotificationServices()
                .AddPolicyBasedApiKeyAuthenticationServices(builder.Configuration["XApiKey"])
                .AddPolicyBasedRoleAuthorizationServices()
                .Configure<PathToBpmnDirectory>(builder.Configuration.GetSection("PathToBpmnDirectory"))
                .Configure<PathToSolutionDirectory>(builder.Configuration.GetSection("PathToSolutionDirectory"))
                .Configure<JavaApiHelper>(builder.Configuration.GetSection("JavaApiHelper"))
                .AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

var app = builder.Build();

app.ConfigureCustomExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
        {
            if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host"))
            {
                return;
            }
            var routePrefix = builder.Configuration.GetValue<string>("RoutePrefix");
            var scheme = builder.Configuration.GetSection("Swagger").GetValue<string>("Schema");
            if (string.IsNullOrWhiteSpace(scheme))
            {
                scheme = "https";
            }
            var serverUrl = $"{scheme}://{httpRequest.Headers["X-Forwarded-Host"]}/{routePrefix}";
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
        });
    })
       .UseSwaggerUI(c =>
       {
           c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
       });
}

app.MapControllers();
app.MapSwagger();
app.UpdateDatabase();
await app.SeedKeywordsDataAsync();

app.MapHub<NotificationHub>("/notification-hub");

app.Run();