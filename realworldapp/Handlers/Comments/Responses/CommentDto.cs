using System;
using realworldapp.Handlers.Users.Responses;
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
            Author = new UserDto(comment.Author);
        }

        public ulong Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public UserDto Author { get; set; }
    }
}