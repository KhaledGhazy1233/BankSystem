using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ImplementServices;
using Domainlayer.BankSystem.Results;
using InfrastructureLayer.BankSystem.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Concurrent;


using System.Text;

namespace ApplicationLayer.BankSystem.ModuleDependences
{
    public static class ModuleServiceDependences
    {
        public static IServiceCollection AddServiceDependences(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind JwtSettings from appsettings.json
            services.Configure<JwtSetting>(configuration.GetSection("jwtSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSetting>>().Value);

            // Resolve jwtSettings instance for token validation
            var jwtSettings = configuration.GetSection("jwtSettings").Get<JwtSetting>();

            // Register custom services
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IFormFileService, FormFileService>();
            services.AddTransient<IAddUserImageService, AddUserImageService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddSingleton(new ConcurrentDictionary<string, RefreshToken>());
            services.AddHttpContextAccessor();
            // Add Authentication with JWT
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),

                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.Audience,

                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ClockSkew = TimeSpan.Zero // مهم علشان يمنع تأخير وقتي
                };
            });

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = " BankSystem", Version = "v1" });
                c.EnableAnnotations();

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("CreateStudent", policy =>
                {
                    policy.RequireClaim("Create Student", "True");
                });
                option.AddPolicy("DeleteStudent", policy =>
                {
                    policy.RequireClaim("Delete Student", "True");
                });
                option.AddPolicy("EditStudent", policy =>
                {
                    policy.RequireClaim("Edit Student", "True");
                });
            });


            return services;
        }
    }
}
