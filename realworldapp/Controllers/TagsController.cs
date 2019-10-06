using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using realworldapp.Handlers.Tags.Commands;
using realworldapp.Handlers.Tags.Response;
using realworldapp.Models;

namespace realworldapp.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public TagsController(AppDbContext context, IMediator mediator)
        {
            _mediator = mediator;
            _context = context;

            if (!_context.Tags.Any())
            {
                _context.Tags.AddRange(new Tag {Name = "tag1"}, new Tag { Name = "tag2" }, new Tag { Name = "tag3" });
                _context.SaveChanges();
            }
        }
        // GET
        [HttpGet]
        public async Task<TagListWrapper> GetTagList()
        {
            return await _mediator.Send(new QueryTagListCommand(), new CancellationToken());
        }
    }
}