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

            //modelBuilder.Entity<Article>(entity =>
            //    {
            //        entity.HasMany(i => i.Comments).WithOne(i => i.Article).HasForeignKey(i => i.Article.ArticleId)
            //            .OnDelete(DeleteBehavior.Cascade);
            //    });

            //modelBuilder.Entity<Comment>(entity =>
            //{
            //    entity.HasOne(item => item.Article).WithMany(relation => relation.Comments)
            //        .HasForeignKey(key => key.Article.ArticleId).OnDelete(DeleteBehavior.Cascade);
            //    entity.HasOne(item => item.Article).WithMany(relation => relation.Comments)
            //        .HasForeignKey(i => i.Author.ProfileId).OnDelete(DeleteBehavior.Restrict);

            //});

            //modelBuilder.Entity<ArticleProfile>(entity =>
            //{
            //    entity.HasKey(key => new
            //    {
            //        key.ProfileId,
            //        key.ArticleId
            //    });

            //    entity.HasOne(item => item.Article).WithMany(relation => relation.FavoritedArticles)
            //        .HasForeignKey(key => key.ArticleId);

            //    entity.HasOne(item => item.Person).WithMany(relation => relation.FavoritedArticles)
            //        .HasForeignKey(key => key.ProfileId);
            //});
        }

    }
}
