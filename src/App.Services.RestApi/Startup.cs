using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Code.Example.App.Services.RestApi
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope? AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiKeyOptions>(Configuration.GetRequiredSection("Feature"));
            services.Configure<FeatureOptions>(Configuration.GetRequiredSection("Feature"));
            services.Configure<RabbitMqOption>(Configuration.GetSection("RabbitMq"));
            services.Configure<LegacyHealthMonitorOption>(Configuration.GetSection("RabbitMq"));
            services.Configure<BareosOptions>(Configuration.GetSection("Bareos"));
            services.Configure<FakeBackupServiceOptions>(Configuration.GetSection("FakeBackupService"));

            services.AddVstackAppModule(Configuration);
            AddApiKeyAuthentication(services);
            AddControllers(services);
            services.AddHttpContextAccessor();
            AddSwaggerGen(services);

            var useRabbitMqPublisher = Configuration.GetSection("RabbitMq").Exists();
            if (useRabbitMqPublisher)
                services.AddHostedService<RabbitMqService>();

            LocationDefiner.Define(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var useRabbitMqPublisher = Configuration.GetSection("RabbitMq").Exists();
            var useMonolithGate = Configuration.GetSection(MonolithGateConfiguration.ConfigKey).Exists();
            builder.RegisterModule(new VstackModule(useRabbitMqPublisher, useMonolithGate));
            builder.RegisterModule<RestApiModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new HttpDiagnosticsSubscriber(),
                new EfCoreDiagnosticsSubscriber());

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            UseSwagger(app);
            UseLogs(app, env);
            app.UseMiddleware<ProxyHandler>();
            app.UseRouting();
            app.UseProjectIdExtractor();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void UseLogs(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ApiRequestLoggingMiddleware>();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseMiddleware<ExceptionHandler>();
        }

        private static void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(o => o.SerializeAsV2 = false);

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v4-admin/swagger.json", "vStack administration API V4");
                c.SwaggerEndpoint("/swagger/v4-finances/swagger.json", "vStack finance API V4");
                c.SwaggerEndpoint("/swagger/v4-projects/swagger.json", "vStack project API V4");
                c.SwaggerEndpoint("/swagger/v4-marketing/swagger.json", "vStack marketing API V4");

                c.RoutePrefix = string.Empty;
            });
        }

        private static void AddApiKeyAuthentication(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, null);

            serviceCollection
                .AddAuthorization(o => o.AddPolicy(AuthorizationPolicies.PartnerScopePolicy, x => x.RequireClaim(CustomClaims.PartnerScopeClaim)));
        }

        private static void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v4-admin", new OpenApiInfo
                {
                    Title = "vStack administrators API",
                    Version = "v4"
                });

                c.SwaggerDoc("v4-finances", new OpenApiInfo
                {
                    Title = "vStack finances API",
                    Version = "v4"
                });

                c.SwaggerDoc("v4-projects", new OpenApiInfo
                {
                    Title = "vStack project API",
                    Version = "v4"
                });

                c.SwaggerDoc("v4-marketing", new OpenApiInfo
                {
                    Title = "vStack marketing API",
                    Version = "v4"
                });

                c.AddSecurityDefinition("api-key", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Client API-KEY",
                    In = ParameterLocation.Header,
                    Name = ApiKeyAuthenticationHandler.ApiKeyHeaderName,
                    Scheme = ApiKeyAuthenticationOptions.DefaultScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "api-key"}
                        },
                        new[] {"readAccess", "writeAccess"}
                    }
                });

                c.EnableAnnotations();
                c.UseAllOfToExtendReferenceSchemas();
                c.SupportNonNullableReferenceTypes();
                c.SchemaFilter<RequiredNotNullableSchemaFilter>();
                c.OperationFilter<PartnerNameHeaderParameter>();
                c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        private static void AddControllers(IServiceCollection services)
        {
            services
                .AddControllers(o =>
                {
                    o.SuppressAsyncSuffixInActionNames = true;

                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser().Build();

                    o.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddNewtonsoftJson(options =>
                {
                    var json = options.SerializerSettings;
                    json.Converters.Add(new StringEnumConverter());
                    json.NullValueHandling = NullValueHandling.Ignore;
                    json.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = false;
                    options.SuppressMapClientErrors = true;
                    options.InvalidModelStateResponseFactory = actionContext
                        => new BadRequestObjectResult(actionContext.ModelState.AsErrors().AsErrorResponse());
                });
        }
    }
}
