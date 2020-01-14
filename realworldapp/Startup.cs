using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using realworldapp.Infrastructure;
using realworldapp.Infrastructure.Security;
using realworldapp.Infrastructure.Security.JWT;
using realworldapp.Infrastructure.Security.Session;
using realworldapp.Models;

namespace realworldapp
{
    public class Startup
    {
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
                opt.UseSqlServer(Configuration.GetConnectionString("RealWorldApp")));
            services.AddMvc(options => { options.Filters.Add(typeof(SessionFilter)); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransactionPipeline();
            services.AddSession();
            services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
            services.AddScoped<IJwt, Jwt>();
            services.AddScoped<ISlugGenerator, SlugGenerator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "muj.pokus.cz",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ReallySecretCode")),
                    ValidateLifetime = true,
                    ValidateAudience = false
                }
            );
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
                    if (true) //!serviceScope.ServiceProvider.GetService<AppDbContext>().AllMigrationsApplied())
                    {
                        // serviceScope.ServiceProvider.GetService<ApiContext>().Database.Migrate();
                        serviceScope.ServiceProvider.GetService<AppDbContext>().EnsureDatabaseSeeded();
                    }
                }
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
        }
    }
}
