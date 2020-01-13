using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace realworldapp.Handlers.Articles
{
    public class UnfavoriteArticleHandler : IRequestHandler<UnfavoriteArticleCommand, ArticleDetailWrapper>
    {
        private readonly AppDbContext _context;

        public UnfavoriteArticleHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleDetailWrapper> Handle(UnfavoriteArticleCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Slug))
            {
                return null; // todo return invalid command
            }

            var article = await _context.Articles.IncludeAllArticleInformationNotTracking()
                .FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);

            if (article == default(Article))
            {
                throw new NotFoundCommandException(new { Article = "not found" });
            }

            var favoritesArticle = article.FavoritedArticles
                .FirstOrDefault(i => i.ArticleId == article.ArticleId && i.ProfileId == _context.UserInfo.ProfileId);

            if (favoritesArticle != null)
            {
                _context.FavoritedArticles.Remove(favoritesArticle);
                await _context.SaveChangesAsync(cancellationToken);
            }

            article = await _context.Articles.IncludeAllArticleInformationNotTracking()
                .FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);

            if (article == default(Article))
            {
                throw new NotFoundCommandException(new { Article = "not found" });
            }

            return new ArticleDetailWrapper(article);
        }

    }
}