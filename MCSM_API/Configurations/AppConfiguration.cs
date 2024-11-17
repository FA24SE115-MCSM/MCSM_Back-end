using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using MCSM_API.Configurations.Middleware;
using MCSM_Data;
using MCSM_Service.Implementations;
using MCSM_Service.Interfaces;
using Microsoft.OpenApi.Models;

namespace MCSM_API.Configurations
{
    public static class AppConfiguration
    {
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRetreatService, RetreatService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IRetreatLessonService, RetreatLessonService>();
            services.AddScoped<IRetreatMonkService, RetreatMonkService>();
            services.AddScoped<IRetreatGroupService, RetreatGroupService>();
            services.AddScoped<IRetreatRegistrationService, RetreatRegistrationService>();
            services.AddScoped<IRetreatRegistrationParticipantService, RetreatRegistrationParticipantService>();
            services.AddScoped<IRetreatRegistrationParticipantService, RetreatRegistrationParticipantService>();
            services.AddScoped<IRetreatRegistrationService, RetreatRegistrationService>();
            services.AddScoped<IToolService, ToolService>();
            services.AddScoped<IToolHistoryService, ToolHistoryService>();
            services.AddScoped<IDeviceTokenService, DeviceTokenService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IRetreatScheduleService, RetreatScheduleService>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IFeedbackService, FeedbackService>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MCSM Service Interface",
                    Description = @"APIs for Application to manage Meditation Center and Practitioner Management System at Plum Village Thailand.
                        <br/>
                        <br/>
                        <strong>WebApp:</strong> <a href='' target='_blank'>https://webapp.mcsm.com</a>",
                    Version = "v1"
                });
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Use the JWT Authorization header with the Bearer scheme. Enter 'Bearer' followed by a space, then your token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                 });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });
        }

        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
        }

        public static void UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }


        public static void AddHangfireDashboard(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire/job-dashboard", new DashboardOptions
            {
                DashboardTitle = "Hangfire Job of Plum-Village server",
                DarkModeEnabled = true,
                IgnoreAntiforgeryToken = true,
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = "admin.mcsm",
                        Pass = "admin.mcsm.@.@"
                    }
                }
            });
        }

        public static void AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    });
            });

            services.AddHangfireServer();
        }

        public static void AddHangfireJobs(this IServiceProvider serviceProvider, IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate(
                "DivideGroupOfRetreat",
                () => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IRetreatGroupService>().DivideGroup(),
                "0 17 * * *"
            );

        }
    }
}
