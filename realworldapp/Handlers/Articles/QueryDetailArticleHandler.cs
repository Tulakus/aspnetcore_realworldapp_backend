using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryDetailArticleHandler : IRequestHandler<QueryDetailArticleCommand, ArticleDetailWrapper>
    {
        private readonly AppDbContext _context;

        public QueryDetailArticleHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleDetailWrapper> Handle(QueryDetailArticleCommand command, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);
            if (article == null)
            {
                throw new NotFoundCommandException(new { Article = "not found" });
            }

            return new ArticleDetailWrapper(article);
        }

    }
}