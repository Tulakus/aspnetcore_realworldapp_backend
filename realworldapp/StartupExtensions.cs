using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using realworldapp.Infrastructure;
using realworldapp.Infrastructure.Security.Jwt;

namespace realworldapp
{
    public static class StartupExtensions
    {
        public static void AddTransactionPipeline(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));
        }

        public static void AddValidationPipeline(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
        }

        public static void AddJwtAuthentication(this IServiceCollection services, string issuer, string secretCode, double tokenExpiration)
        {
            services.Configure<JwtSettings>(i =>
            {
                i.Issuer = issuer;
                i.SecretCode = secretCode;
                i.TokenExpiration = tokenExpiration;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = Jwt.GetSymmetricSecurityKey(secretCode),
                        ValidateLifetime = true,
                        ValidateAudience = false
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            var token = context.HttpContext.Request.Headers["Authorization"];

                            // RealWorldApi spec expect token set as Token <valid_token> instead Bearer <valid_token>
                            if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0].Substring("Token ".Length).Trim();
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );


        }
    }

}