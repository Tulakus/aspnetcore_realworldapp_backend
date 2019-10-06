using realworldapp.Models;

namespace realworldapp.Handlers.Comments.Responses
{
    public class CommentWrapper
    {
        public CommentWrapper(Comment comment)
        {
            Comment = new CommentDto(comment);
        }
        public CommentDto Comment { get; set; }
    }
}