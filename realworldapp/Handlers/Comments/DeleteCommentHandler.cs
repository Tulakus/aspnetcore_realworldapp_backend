using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Comments.Commands;
using realworldapp.Handlers.Comments.Responses;
using realworldapp.Models;

namespace realworldapp.Handlers.Comments
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, CommentWrapper>
    {
        private readonly AppDbContext _context;

        public DeleteCommentHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CommentWrapper> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
             var queryable = _context.Comments.Include(i => i.Article).Include(i => i.Author);

            var comment = await queryable.FirstOrDefaultAsync(
                i => i.CommentId == command.CommentId && i.Article.Slug == command.Slug, cancellationToken);

            if(comment == default(Comment))
            {
                return null; //todo
            }

            if (comment.Author.ProfileId != _context.UserInfo.ProfileId ||
                comment.Article.Author.ProfileId != _context.UserInfo.ProfileId)
            {
                return null; // todo unauthorized
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return null; // todo new CommentWrapper(newComment);
        }
    }
}