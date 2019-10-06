using System.Collections.Generic;
using System.Linq;
using realworldapp.Models;

namespace realworldapp.Handlers.Comments.Responses
{
    public class CommentListWrapper
    {
        public CommentListWrapper(IList<Comment> comments)
        {
            Comments = comments.Select(i => new CommentDto(i)).ToList();
        }
        public IList<CommentDto> Comments { get; set; }
    }
}