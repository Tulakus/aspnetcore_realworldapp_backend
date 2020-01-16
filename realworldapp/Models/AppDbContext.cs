using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using realworldapp.Infrastructure.Security.Session;

namespace realworldapp.Models
{
    public class AppDbContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public UserInfo UserInfo { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<UserFollower> Followers { get; set; }
        public DbSet<ArticleProfile> FavoritedArticles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTag>(entity =>
            {
                entity.HasKey(key => new
                {
                    key.TagId,
                    key.ArticleId
                });

                entity.HasOne(item => item.Article).WithMany(relation => relation.ArticleTags)
                    .HasForeignKey(key => key.ArticleId);

                entity.HasOne(item => item.Tag).WithMany(relation => relation.ArticleTags)
                    .HasForeignKey(key => key.TagId);
            });

            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasKey(key => new
                {
                    key.FollowedId,
                    key.FollowingId
                });

                entity.HasOne(item => item.Followed).WithMany(relation => relation.Followers)
                    .HasForeignKey(key => key.FollowedId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(item => item.Following).WithMany(relation => relation.Following)
                    .HasForeignKey(key => key.FollowingId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<ArticleProfile>(entity =>
            {
                entity.HasKey(key => new
                {
                    key.ArticleId,
                    key.ProfileId
                });

                entity.HasOne(item => item.Article).WithMany(relation => relation.FavoritedArticles)
                    .HasForeignKey(key => key.ProfileId);
                entity.HasOne(item => item.Person).WithMany(relation => relation.FavoritedArticles)
                    .HasForeignKey(key => key.ArticleId);
            });
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null || Database.IsInMemory())
            {
                return;
            }
            
            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);

        }

        public void CommitTransaction()
        {
            try
            {
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }
}
