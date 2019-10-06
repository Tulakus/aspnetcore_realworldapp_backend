using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Models;

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
            var article = await _context.Articles.Include(i => i.ArticleTags).ThenInclude(i => i.Tag).FirstAsync(cancellationToken);
            if (article == null)
            {
                throw new NotFoundCommandException(new { Article = "not found" });
            }
            
            _context.ArticleTags.RemoveRange(article.ArticleTags);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}