using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using realworldapp.Infrastructure.Security.Session;
using realworldapp.Models;

namespace realworldapp.Models
{
    public class AppDbContext : DbContext
    {
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
        public DbSet<ArticleUserFavorited> ArticlesFavoritedBy { get; set; }

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
                    key.FollowerId
                });

                entity.HasOne(item => item.Followed).WithMany(relation => relation.FollowedPeople)
                    .HasForeignKey(key => key.FollowedId);

                entity.HasOne(item => item.Follower).WithMany(relation => relation.Followers)
                    .HasForeignKey(key => key.FollowerId);

            });

            modelBuilder.Entity<ArticleUserFavorited>(entity =>
            {
                entity.HasKey(key => new
                {
                    key.ArticleId,
                    key.ProfileId
                });

                entity.HasOne(item => item.Article).WithMany(relation => relation.FavoritedByUser).HasForeignKey(key => key.ProfileId);
                entity.HasOne(item => item.FavoritedBy).WithMany(relation => relation.FavoritedArticles).HasForeignKey(key => key.ArticleId);
            });
        }
        public DbSet<Comment> Comments { get; set; }
    }
}
