using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var article = await _context.Articles
                .FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);

            if (article == null)
            {
                throw new NotFoundCommandException(new { Article = "not found" });
            }

            if(article.Author.ProfileId != _context.UserInfo.ProfileId)
            {
                throw new NotFoundCommandException(new { Article = "not found" }); // todo unauthorized
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}