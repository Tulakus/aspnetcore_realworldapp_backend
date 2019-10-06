using MediatR;
using realworldapp.Handlers.Comments.Responses;

namespace realworldapp.Handlers.Comments.Commands
{
    public class CreateCommentCommand : IRequest<CommentWrapper>
    {
        public CommentData Comment { get; set; }
        public string Slug { get; set; }
    }

    public class CommentData
    {
        public string Body { get; set; }
    }
}