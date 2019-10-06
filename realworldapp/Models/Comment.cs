using System;

namespace realworldapp.Models
{
    public class Comment
    {
        public ulong CommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public User Author { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}