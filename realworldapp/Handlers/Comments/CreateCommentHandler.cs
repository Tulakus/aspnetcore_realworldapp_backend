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
    public class CreateCommentHandler: IRequestHandler<CreateCommentCommand, CommentWrapper>
    {
        private readonly AppDbContext _context;

        public CreateCommentHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CommentWrapper> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Slug))
                return null;

            //var article = await _context.Articles.FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);
            var article = await _context.Articles.FirstAsync(cancellationToken);

            if (article == default(Article))
                return null;

            if (command.Comment == default(CommentData) || string.IsNullOrEmpty(command.Comment.Body))
                return null;
            var author = await _context.Users.FirstAsync(cancellationToken);

            var newComment = new Comment
            {
                Body = command.Comment.Body,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Article = article,
                Author = author

            };

            _context.Comments.Add(newComment);

            await _context.SaveChangesAsync(cancellationToken);

            return new CommentWrapper(newComment);
        }
    }
}