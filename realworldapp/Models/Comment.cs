using System;

namespace realworldapp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public int ProfileId { get; set; }
        public Profile Author { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}