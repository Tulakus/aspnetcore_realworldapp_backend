using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticleHandler : IRequestHandler<QueryArticlesCommand, ArticleListWrapper>
    {
        private AppDbContext _context;
        private readonly HttpContext _httpContext;

        public QueryArticleHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<ArticleListWrapper> Handle(QueryArticlesCommand command, CancellationToken cancellationToken)
        {
            var articles = _context.Articles.Include(i => i.ArticleTags).ThenInclude(i => i.Tag);

            var request = _httpContext.Request;
            var tag = request.Query["tag"].ToString();

            var author = request.Query["author"].ToString();
            var favorited = request.Query["favorited"].ToString();

            int.TryParse(request.Query["limit"], out var limit);
            limit = limit == 0 ? 20 : limit;
            int.TryParse(request.Query["offset"], out var offset);
            
            return new ArticleListWrapper(articles);
        }

    }
}