using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticlesHandler : IRequestHandler<QueryArticlesCommand, ArticleListWrapper>, IRequestHandler<QueryArticleCommand, ArticleDetailWrapper>
    {
        private readonly AppDbContext _context;
        private  const int DefaultLimit = 20;
        private  const int DefaultOffset = 0;
        public QueryArticlesHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleListWrapper> Handle(QueryArticlesCommand command, CancellationToken cancellationToken)
        {
            var articleQueryable = _context.Articles
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.FavoritedArticles)
                .Include(i => i.Author)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(command.Author))
            {
                if (!await _context.Profiles.AnyAsync(i => i.Username == command.Author, cancellationToken))
                {
                    return GetDefaultResponse();
                }

                articleQueryable = articleQueryable.Where(i => i.Author.Username == command.Author);
            }

            if (!string.IsNullOrWhiteSpace(command.Tag))
            {
                articleQueryable = articleQueryable.Where(article => article.ArticleTags.Any(tag => tag.Tag.Name == command.Tag));
            }

            if (!string.IsNullOrWhiteSpace(command.Favorited))
            {
                articleQueryable = articleQueryable.Where(article => article.FavoritedArticles
                    .Any(users => users.Person.Username == command.Favorited));
            }

            var result = await articleQueryable.OrderByDescending(i => i.CreatedAt)
                .Skip(command.Offset == default ? DefaultOffset : command.Offset)
                .Take(command.Limit == default ? DefaultLimit : command.Limit)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new ArticleListWrapper(result);
        }

        ArticleListWrapper GetDefaultResponse()
        {
            return new ArticleListWrapper(new Article[0]); // todo error message -> not found
        }

        public async Task<ArticleDetailWrapper> Handle(QueryArticleCommand request, CancellationToken cancellationToken)
        {
            var articleQueryable = _context.Articles
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.FavoritedArticles)
                .Include(i => i.Author)
                .AsNoTracking();

            if (string.IsNullOrWhiteSpace(request.Slug))
            {
                return null; // todo
            }

            var result = await articleQueryable.FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken);

            return new ArticleDetailWrapper(result);
        }
    }
}