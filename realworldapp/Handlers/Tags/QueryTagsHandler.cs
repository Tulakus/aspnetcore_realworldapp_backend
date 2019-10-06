using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Tags.Commands;
using realworldapp.Handlers.Tags.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Tags
{
    public class QueryTagsHandler: IRequestHandler<QueryTagListCommand, TagListWrapper>
    {
        private AppDbContext _context;

        public QueryTagsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TagListWrapper> Handle(QueryTagListCommand command, CancellationToken cancellationToken)
        {
            var tagList = await _context.Tags.ToListAsync(cancellationToken);
            var dto = tagList.Select(i => i.Name).ToList();
            return new TagListWrapper
            {
                Tags = dto
            };
        }
    }
}