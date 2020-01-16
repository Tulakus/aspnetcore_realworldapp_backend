using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using realworldapp.Infrastructure.Security.Session;
using realworldapp.Models;

namespace realworldapp
{
    public static class DbContextExtensions
    {

        public static IQueryable<Article> IncludeAllArticleInformation(this DbSet<Article> article)
        {
            return article.Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.FavoritedArticles)
                .Include(i => i.Author);
        }

        public static IQueryable<Article> IncludeAllArticleInformationNotTracking(this DbSet<Article> article)
        {
            return article.Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.FavoritedArticles)
                .Include(i => i.Author)
                .AsNoTracking();
        }

        public static Task<Profile> GetProfile(this DbSet<Profile> dtb, UserInfo userInfo, CancellationToken token)
        {
            return dtb.FirstAsync(i => i.Username == userInfo.Username, token);
        }

        public static void EnsureDatabaseSeeded(this AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>();
                for (var i = 1; i <= 5; i++)
                {
                    users.Add(
                        new User()
                        {
                            Login = $"user{i}@user.com",
                            Password = "password",
                            Profile = new Profile()
                            {
                                Username = $"user{i}"
                            }
                        });
                }
                context.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Followers.Any())
            {
                context.AddRange(new List<UserFollower>{
                    new UserFollower()
                    {
                        FollowingId = 1,
                        FollowedId = 2
                    },
                    new UserFollower()
                    {
                        FollowingId = 1,
                        FollowedId = 3
                    },
                    new UserFollower()
                    {
                        FollowingId = 2,
                        FollowedId = 1
                    },
                    new UserFollower()
                    {
                        FollowingId = 4,
                        FollowedId = 5
                    },
                });
            }

            if (!context.Tags.Any())
            {
                var tagNames = new[] { "C#", "Javascript", "Typescript", "React", "Programming", "Clean Code" };
                var tags = tagNames.Select(name => new Tag()
                {
                    Name = name
                });
                context.AddRange(tags);
                context.SaveChanges();
            }

            if (!context.Articles.Any())
            {
                var articles = new List<Article>()
                {
                    new Article()
                    {
                        Body = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                        Author = new Profile(){ProfileId = 1},
                        Title = "Title 1",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Article()
                    {
                        Body = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                        Author = new Profile(){ProfileId = 1},
                        Title = "Title 2",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Article()
                    {
                        Body = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                        Author = new Profile(){ProfileId = 2},
                        Title = "Title 1",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Article()
                    {
                        Body = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
                        Author = new Profile(){ProfileId = 3},
                        Title = "Title 1",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.AddRange(articles);
                context.SaveChanges();
            }

            if (!context.ArticleTags.Any())
            {
                var articleTags = new List<ArticleTag>()
                {
                    new ArticleTag()
                    {
                        ArticleId = 1,
                        TagId = 1
                    },
                    new ArticleTag()
                    {
                        ArticleId = 1,
                        TagId = 2
                    },
                    new ArticleTag()
                    {
                        ArticleId = 1,
                        TagId = 3
                    }
                };

                context.AddRange(articleTags);
                context.SaveChanges();
            }
        }
    }
}
