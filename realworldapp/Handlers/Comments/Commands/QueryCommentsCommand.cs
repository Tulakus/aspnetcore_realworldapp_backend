using MediatR;
using realworldapp.Handlers.Comments.Responses;

namespace realworldapp.Handlers.Comments.Commands
{
    public class QueryCommentsCommand : IRequest<CommentListWrapper>
    {
        public QueryCommentsCommand(string slug)
        {
            Slug = slug;
        }
        public string Slug { get; set; }
    }
}