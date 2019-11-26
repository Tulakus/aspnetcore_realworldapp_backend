﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace realworldapp.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        [JsonIgnore]
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Favorited { get; set; }
        public uint FavoritesCount { get; set; }

        public int UserId { get; set; }
        public User Author { get; set; } 
        public ICollection<Comment> Comments { get; set; }
    }
}