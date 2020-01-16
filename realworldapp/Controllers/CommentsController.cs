using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using realworldapp.Handlers.Comments.Commands;
using realworldapp.Handlers.Comments.Responses;

namespace realworldapp.Controllers
{
    [Route("api/articles/{slug}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<CommentListWrapper> GetComment([FromRoute] string slug)
        {
            return await _mediator.Send(new QueryCommentsCommand(slug));
        }

        [HttpPost]
        public async Task<CommentWrapper> PostComment([FromBody] CreateCommentCommand command, [FromRoute] string slug)
        {
            command.Slug = slug;
            return await _mediator.Send(command);
        }

        [HttpDelete("{commentId}")]
        public async Task<Unit> DeleteComment([FromRoute] string slug, [FromRoute] int commentId)
        {
            return await _mediator.Send(new DeleteCommentCommand() { CommentId = commentId, Slug = slug });
        }

    }
}