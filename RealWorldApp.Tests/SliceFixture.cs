using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using realworldapp;
using realworldapp.Models;
using Respawn;

//Based on https://github.com/jbogard/ContosoUniversityDotNetCore-Pages/blob/master/ContosoUniversity.IntegrationTests/SliceFixture.cs
namespace RealWorldApp.IntegrationTests
{
    public class SliceFixture
    {
        private static readonly Checkpoint Checkpoint;
        private static readonly IConfigurationRoot Configuration;
        private static readonly IServiceScopeFactory ScopeFactory;
        
        static SliceFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var startup = new Startup(Configuration);
            var services = new ServiceCollection();
            services.AddLogging();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            ScopeFactory = provider.GetService<IServiceScopeFactory>();
            Checkpoint = new Checkpoint();
        }

        public static Task ResetCheckpoint() => Checkpoint.Reset(Configuration.GetConnectionString(Startup.ApplicationDatabaseKey));

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

                try
                {
                    await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                    await action(scope.ServiceProvider).ConfigureAwait(false);

                    await dbContext.CommitTransactionAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

                try
                {
                    await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                    var result = await action(scope.ServiceProvider).ConfigureAwait(false);

                    await dbContext.CommitTransactionAsync().ConfigureAwait(false);

                    return result;
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }


        public static Task ExecuteDbContextAsync(Func<AppDbContext, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));

        public static Task ExecuteDbContextAsync(Func<AppDbContext, ValueTask> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()).AsTask());

        public static Task ExecuteDbContextAsync(Func<AppDbContext, IMediator, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>(), sp.GetService<IMediator>()));

        public static Task<T> ExecuteDbContextAsync<T>(Func<AppDbContext, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()));

        public static Task<T> ExecuteDbContextAsync<T>(Func<AppDbContext, ValueTask<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>()).AsTask());

        public static Task<T> ExecuteDbContextAsync<T>(Func<AppDbContext, IMediator, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<AppDbContext>(), sp.GetService<IMediator>()));

        public static Task InsertAsync<T>(params T[] entities) where T : class
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Set<T>().Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
            where TEntity : class
            where TEntity2 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
            where TEntity : class
            where TEntity2 : class
            where TEntity3 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3, TEntity4 entity4)
            where TEntity : class
            where TEntity2 : class
            where TEntity3 : class
            where TEntity4 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);
                db.Set<TEntity4>().Add(entity4);

                return db.SaveChangesAsync();
            });
        }

        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        private static int _courseNumber = 1;

        public static int NextCourseNumber() => Interlocked.Increment(ref _courseNumber);

    }
}