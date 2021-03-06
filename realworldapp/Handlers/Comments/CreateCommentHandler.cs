﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Handlers.Comments.Commands;
using realworldapp.Handlers.Comments.Responses;
using realworldapp.Infrastructure;
using realworldapp.Models;

namespace realworldapp.Handlers.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentWrapper>
    {
        private readonly AppDbContext _context;

        public CreateCommentHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CommentWrapper> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.Include(i => i.Comments)
                .FirstOrDefaultAsync(i => i.Slug == command.Slug, cancellationToken);

            if (article == default(Article))
            {
                throw new NotFoundCommandException(new { Article = ErrorMessages.NotFound });
            }

            var author = await _context.Profiles.GetProfile(_context.UserInfo, cancellationToken);

            var newComment = new Comment
            {
                Body = command.Comment.Body,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Author = author,
                Article = article
            };

            await _context.Comments.AddAsync(newComment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CommentWrapper(newComment);
        }
    }
}