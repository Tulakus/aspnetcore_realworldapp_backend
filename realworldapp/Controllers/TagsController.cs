using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using realworldapp.Handlers.Tags.Commands;
using realworldapp.Handlers.Tags.Response;

namespace realworldapp.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<TagListWrapper> GetTagList()
        {
            return await _mediator.Send(new QueryTagListCommand());
        }
    }
}