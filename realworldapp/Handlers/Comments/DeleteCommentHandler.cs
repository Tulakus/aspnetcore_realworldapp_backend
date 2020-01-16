using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Handlers.Comments.Commands;
using realworldapp.Infrastructure;
using realworldapp.Models;

namespace realworldapp.Handlers.Comments
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly AppDbContext _context;

        public DeleteCommentHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            var queryable = _context.Comments.Include(i => i.Article).Include(i => i.Author);

            var comment = await queryable.FirstOrDefaultAsync(
                i => i.CommentId == command.CommentId && i.Article.Slug == command.Slug, cancellationToken);

            if (comment == default(Comment))
            {
                throw new NotFoundCommandException(new { Article = ErrorMessages.NotFound });
            }

            var isArticleAuthor = comment.Author.ProfileId == _context.UserInfo.ProfileId;
            var isCommentAuthor = comment.Article.Author.ProfileId == _context.UserInfo.ProfileId;

            if (!isCommentAuthor || !isArticleAuthor)
            {
                throw new ForbiddenCommandException(new { Comment = ErrorMessages.DeleteByNotAuthor });
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}