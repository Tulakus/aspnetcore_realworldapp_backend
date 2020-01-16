using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Models;
using System.Threading;
using System.Threading.Tasks;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Infrastructure;

namespace realworldapp.Handlers.Articles
{
    public class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand, Unit>
    {
        private AppDbContext _context;

        public DeleteArticleHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.Include(i => i.Author)
                .FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);

            if (article == null)
            {
                throw new NotFoundCommandException(new { Article = ErrorMessages.NotFound });
            }

            if (article.Author.ProfileId != _context.UserInfo.ProfileId)
            {
                throw new ForbiddenCommandException(new { Article = ErrorMessages.DeleteByNotAuthor});
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}