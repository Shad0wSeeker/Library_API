using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Infrastructure;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LibraryAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLibraryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });


                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "Enter your token in the format `{token}`"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString();
                        if (token.StartsWith("Bearer "))
                        {
                            token = token.Substring("Bearer ".Length).Trim();
                        }
                        context.Token = token;
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRole.Admin.ToString()));
                options.AddPolicy("ClientPolicy", policy => policy.RequireRole(UserRole.Client.ToString()));
                options.AddPolicy("AdminAndClientPolicy", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole(UserRole.Admin.ToString()) ||
                    context.User.IsInRole(UserRole.Client.ToString())));
            });

            return services;
        }
    }
}
