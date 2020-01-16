using System;
using System.Linq;
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
    public class QueryCommentsHandler : IRequestHandler<QueryCommentsCommand, CommentListWrapper>
    {
        private readonly AppDbContext _context;

        public QueryCommentsHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CommentListWrapper> Handle(QueryCommentsCommand command, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.FirstAsync(cancellationToken);

            if (article == default(Article))
                throw new NotFoundCommandException(new {Article = ErrorMessages.NotFound});

            var result = _context.Comments.Include(i => i.Article).ThenInclude(i => i.Author).Where(i => i.Article.Slug == command.Slug).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            return new CommentListWrapper(result);
        }
    }
}