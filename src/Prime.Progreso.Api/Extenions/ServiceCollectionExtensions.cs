using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Api.Policies.ApiKeyPolicy;
using Prime.Progreso.Api.Policies.RolePolicy;
using Prime.Progreso.Data;
using Prime.Progreso.Data.Mapper;
using Prime.Progreso.Data.Seeder.Initializer;
using Prime.Progreso.Domain.Abstractions.Seeders;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Services;
using System.Net;

namespace Prime.Progreso.Api.Extenions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProgresoDbContext(this IServiceCollection services, string connString)
            => services.AddDbContext<ProgresoDbContext>(opt =>
            {
                opt.UseSqlServer(connString);
            });

        public static IServiceCollection AddDbInitializer(this IServiceCollection services)
            => services.AddSingleton<IDbInitializer, DbInitializer>();

        public static IServiceCollection AddProgresoAutomapper(this IServiceCollection services)
            => services.AddAutoMapper(mc =>
                {
                    mc.AddProfile(new MapperProfile());
                    mc.AddProfile(new ActivityProfile());
                    mc.AddProfile(new MilestoneProfile());
                    mc.AddProfile(new CurriculumItemProfile());
                    mc.AddProfile(new CurriculumProfile());
                    mc.AddProfile(new ProjectProfile());
                    mc.AddProfile(new QuestionCategoryProfile());
                    mc.AddProfile(new CodingChallengeProfile());
                    mc.AddProfile(new QuestionCategoryProfile());
                    mc.AddProfile(new AnswerProfile());
                    mc.AddProfile(new QuestionProfile());
                    mc.AddProfile(new KeywordDescriptionProfile());
                    mc.AddProfile(new KeywordProfile());
                    mc.AddProfile(new LanguageProfile());
                    mc.AddProfile(new QuizProfile());
                    mc.AddProfile(new TechnologyProfile());
                    mc.AddProfile(new QuizExecutionProfile());
                    mc.AddProfile(new QuizAssignmentProfile());
                    mc.AddProfile(new AnswerChoiceProfile());
                    mc.AddProfile(new BpmnDiagramProfile());
                    mc.AddProfile(new AssignmentToCodingChallengeProfile());
                    mc.AddProfile(new TestCaseProfile());
                    mc.AddProfile(new KeywordSinglePlayerResultProfile());
                    mc.AddProfile(new KeywordDescriptionSinglePlayerResultProfile());
                    mc.AddProfile(new RandomKeywordDescriptionProfile());
                    mc.AddProfile(new KeywordDescriptionMultiPlayerResultProfile());
                    mc.AddProfile(new KeywordMultiPlayerResultProfile());
                    mc.AddProfile(new SolutionProfile());
                });

        public static IServiceCollection AddProgresoSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Progreso API", Version = "v1" });
                c.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
                {
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme",
                    In = ParameterLocation.Header,
                    Description = "ApiKey must appear in header"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-API-KEY"
                            },
                            In = ParameterLocation.Header
                        },
                        new string[]{}
                    }
                });
            });
        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddTransient<INotificationService, NotificationService>();
            return services;
        }

        public static IServiceCollection AddPolicyBasedApiKeyAuthenticationServices(this IServiceCollection services, string key)
        {
            services.AddTransient<IAuthorizationHandler, ApiKeyAccessRequirementHandler>();
            //services.AddSingleton<IAuthorizationPolicyProvider, ApiKeyPolicyProvider>();
            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy("ApiKeyPolicy",
                    policyBuilder => policyBuilder
                        .AddRequirements(new ApiKeyAccessRequirement(new[] { key })));
            });
            return services;
        }

        public static IServiceCollection AddPolicyBasedRoleAuthorizationServices(this IServiceCollection services)
         {
            services.AddTransient<IAuthorizationHandler, ValidRoleHandler>();
            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy(PolicyConstants.AllowAdminRole,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(RoleAuthorizationConstants.Admin)));

                authConfig.AddPolicy(PolicyConstants.AllowAdminMentorRoles,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(RoleAuthorizationConstants.Admin,
                        RoleAuthorizationConstants.Mentor)));

                authConfig.AddPolicy(PolicyConstants.AllowAdminInternRoles,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(RoleAuthorizationConstants.Admin,
                        RoleAuthorizationConstants.Intern)));

                authConfig.AddPolicy(PolicyConstants.AllowAll,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(RoleAuthorizationConstants.Admin,
                        RoleAuthorizationConstants.Mentor,RoleAuthorizationConstants.Intern)));
            });
            return services;
        }

        public static IServiceCollection ConfigureCustomModelStateResponseFactory(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errorMessages = actionContext.ModelState.Values
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage);

                    return new BadRequestObjectResult(new ProblemDetails()
                    {
                        Type = "about:blank",
                        Title = HttpStatusCode.BadRequest.ToString(),
                        Status = (int)HttpStatusCode.BadRequest,
                        Detail = string.Join('\n', errorMessages),
                        Instance = actionContext.HttpContext.Request.Path
                    });
                };
            });
            return services;
        }
    }
}
