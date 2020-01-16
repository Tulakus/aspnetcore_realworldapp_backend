using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Comments.Commands;
using realworldapp.Handlers.Comments.Responses;
using realworldapp.Models;

namespace realworldapp.Controllers
{
    [Route("api/articles/{slug}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public CommentsController(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<CommentListWrapper> GetComment([FromRoute] string slug)
        {
            return await _mediator.Send(new QueryCommentsCommand(slug));
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<CommentWrapper> PostComment([FromBody] CreateCommentCommand command, [FromRoute] string slug)
        {
            command.Slug = slug;
            return await _mediator.Send(command, new CancellationToken());
        }

        // DELETE: api/Comments/5
        [HttpDelete("{commentId}")]
        public async Task<Unit> DeleteComment([FromRoute] string slug, [FromRoute] int commentId)
        {
            return await _mediator.Send(new DeleteCommentCommand(){CommentId = commentId, Slug = slug}, new CancellationToken());
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}