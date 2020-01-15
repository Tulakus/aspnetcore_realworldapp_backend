using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticleFeedHandler : IRequestHandler<QueryArticlesFeedCommand, ArticleListWrapper>
    {
        private readonly AppDbContext _context;
        private  const int DefaultLimit = 20;
        private  const int DefaultOffset = 0;
        public QueryArticleFeedHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleListWrapper> Handle(QueryArticlesFeedCommand command, CancellationToken cancellationToken)
        {
            var articleQueryable = _context.Articles
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.FavoritedArticles)
                .Include(i => i.Author)
                .AsNoTracking();

            var currentUser = await _context.Profiles.Include(i => i.Following)
                .FirstOrDefaultAsync(i => i.Username == _context.UserInfo.Username, cancellationToken);
            
            if (currentUser == null)
                return null; // todo add error

            var followedPeople = currentUser.Following.Select(i => i.FollowedId).ToList();

            articleQueryable =
                articleQueryable.Where(article => followedPeople.Any(followed => followed == article.Author.ProfileId));
            
            var result = await articleQueryable.OrderByDescending(i => i.CreatedAt)
                .Skip(command.Offset == default ? DefaultOffset : command.Offset)
                .Take(command.Limit == default ? DefaultLimit : command.Limit)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            return new ArticleListWrapper(result);
        }
    }
}