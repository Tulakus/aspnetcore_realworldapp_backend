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
    public class FavoriteArticleHandler : IRequestHandler<FavoriteArticleCommand, ArticleDetailWrapper>
    {
        private readonly AppDbContext _context;

        public FavoriteArticleHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleDetailWrapper> Handle(FavoriteArticleCommand command, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.IncludeAllArticleInformationNotTracking()
                .FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);

            if (article == default(Article))
            {
                throw new NotFoundCommandException(new { Article = "not found" });
            }

            var queryable = _context.FavoritedArticles.Include(i => i.Article).Include(i => i.Person);
            var articleAlreadyFavorites = await queryable.FirstOrDefaultAsync(i =>
                i.ArticleId == article.ArticleId && i.ProfileId == _context.UserInfo.ProfileId, cancellationToken) != null;

            if (!articleAlreadyFavorites)
            {
                var articleFavorites = new ArticleProfile()
                {
                    ArticleId = article.ArticleId,
                    ProfileId = _context.UserInfo.ProfileId
                };

                _context.FavoritedArticles.Add(articleFavorites);
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