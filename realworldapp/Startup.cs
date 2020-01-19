using System;
using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using realworldapp.Infrastructure;
using realworldapp.Infrastructure.Security;
using realworldapp.Infrastructure.Security.Jwt;
using realworldapp.Infrastructure.Security.Password;
using realworldapp.Infrastructure.Security.Session;
using realworldapp.Infrastructure.Security.slug;
using realworldapp.Models;

namespace realworldapp
{
    public class Startup
    {
        public const string JwtTokenExpirationMinutes = "JwtTokenExpirationMinutes";
        public const string ResourcePath = "Resources";
        public const string SeedDatabaseKey = "SeedDatabase";
        public const string ApplicationDatabaseKey = "RealWorldAppDtb";
        public const string JwtSecretCodeKey = "JwtSecretCode";
        public const string JwtIssuerKey = "JwtIssuer";
        public const string DatabaseTypeKey = "DatabaseType";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString(ApplicationDatabaseKey) ?? "Filename=realworld.db";
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContextPool<AppDbContext>(opt =>
            {
                var databaseType = Configuration.GetValue<string>(DatabaseTypeKey)?.ToLower().Trim() ?? "sqlite";
                switch(databaseType)
                {
                    case "sqlite":
                        opt.UseSqlite(connectionString);
                        break;
                    case "sqlserver":
                        opt.UseSqlServer(connectionString);
                        break;
                    default:
                        throw new Exception("Database provider unknown. Please check configuration");
                }

            });
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(SessionFilter));
                    options.Filters.Add(typeof(ValidatorActionFilter));
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    });
            services.AddOptions();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidationPipeline();
            services.AddTransactionPipeline();
            services.AddSession();
            services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
            services.AddScoped<IJwt, Jwt>();
            services.AddScoped(typeof(JwtSettings), typeof(JwtSettings));
            services.AddScoped<ISlugGenerator, SlugGenerator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var jwtIssuer = Configuration.GetValue<string>(JwtIssuerKey);
            var jwtSecretCode = Configuration.GetValue<string>(JwtSecretCodeKey);
            var jwtTokenExpirationMinutes = Configuration.GetValue<double>(JwtTokenExpirationMinutes);

            services.AddJwtAuthentication(jwtIssuer, jwtSecretCode, jwtTokenExpirationMinutes);
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddLocalization(x => x.ResourcesPath = ResourcePath);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
        }
    }
}
