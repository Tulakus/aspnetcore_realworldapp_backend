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
    }
}
