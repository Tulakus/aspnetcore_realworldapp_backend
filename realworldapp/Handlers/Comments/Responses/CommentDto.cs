using System;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Comments.Responses
{
    public class CommentDto
    {
        public CommentDto(Comment comment)
        {
            Body = comment.Body;
            CreatedAt = comment.CreatedAt;
            UpdatedAt = comment.UpdatedAt;
            Id = comment.CommentId;
            Author = new ProfileDto(comment.Author);
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public ProfileDto Author { get; set; }
    }
}