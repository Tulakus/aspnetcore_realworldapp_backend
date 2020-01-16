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
        private string JwtTokenExpirationMinutes = "JwtTokenExpirationMinutes";
        private const string ResourcePath = "Resources";
        private const string SeedDatabaseKey = "SeedDatabase";
        private const string ApplicationDatabaseKey = "RealWorldAppDtb";
        private const string JwtSecretCodeKey = "JwtSecretCode";
        public const string JwtIssuerKey = "JwtIssuer";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContextPool<AppDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString(ApplicationDatabaseKey)));
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
            services.AddJwtAuthentication(Configuration.GetSection(JwtIssuerKey).Value,
                Configuration.GetSection(JwtSecretCodeKey).Value,
                double.Parse(Configuration.GetSection(JwtTokenExpirationMinutes).Value));
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
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    if (bool.Parse(Configuration.GetSection(SeedDatabaseKey).Value))
                    {
                        serviceScope.ServiceProvider.GetService<AppDbContext>().EnsureDatabaseSeeded();
                    }
                }
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
