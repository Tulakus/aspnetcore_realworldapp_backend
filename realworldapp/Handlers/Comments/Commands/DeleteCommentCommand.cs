using MediatR;
using realworldapp.Handlers.Comments.Responses;

namespace realworldapp.Handlers.Comments.Commands
{
    public class DeleteCommentCommand : IRequest<CommentWrapper>
    {
        public int CommentId { get; set; }
        public string Slug { get; set; }
    }
}