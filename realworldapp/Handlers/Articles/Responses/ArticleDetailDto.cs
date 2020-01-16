using System;
using System.Collections.Generic;
using System.Linq;
using realworldapp.Handlers.Tags.Response;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Models
{
    public class ArticleDetailDto
    {
        public ArticleDetailDto(Article article)
        {
            Author = new ProfileDto(article.Author);
            Description = article.Description;
            Title = article.Title;
            Body = article.Body;
            CreatedAt = article.CreatedAt;
            UpdatedAt = article.UpdatedAt;
            Slug = article.Slug;
            TagList = article.ArticleTags.Select(i => i.Tag.Name).ToList();
            Favorited = article.Favorited;
            FavoritesCount = article.FavoritedArticles?.Count ?? 0;
        }

        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public IList<string> TagList { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Favorited { get; set; }
        public int FavoritesCount { get; set; }
        public ProfileDto Author { get; set; }

    }
}