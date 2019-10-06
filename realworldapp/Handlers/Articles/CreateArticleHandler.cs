using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class CreateArticleHandler : IRequestHandler<CreateArticleCommand, ArticleDetailWrapper>
    {
        private AppDbContext _context;

        public CreateArticleHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleDetailWrapper> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
        {
            var commandArticle = command.Article;
            var articleTagList = commandArticle.TagList;
            var newArticle = new Article();
            var articleTags = new List<ArticleTag>();

            if (articleTagList != null && articleTagList.Any())
            {
                

                foreach (string articleTag in articleTagList)
                {
                    var tag = _context.Tags.FirstOrDefault(i => i.Name == articleTag);
                    if (tag == default(Tag))
                    {
                        tag = new Tag
                        {
                            Name = articleTag
                        };

                        _context.Tags.Add(tag);
                        _context.SaveChanges();
                    }

                    articleTags.Add(new ArticleTag
                    {
                        Article = newArticle,
                        Tag = tag
                    });
                }

                
            }

            var author = await _context.Users.FirstAsync(cancellationToken);

            newArticle.Title = commandArticle.Title;
            newArticle.Description = commandArticle.Description;
            newArticle.Body = commandArticle.Body;
            newArticle.CreatedAt = DateTime.UtcNow;
            newArticle.UpdatedAt = DateTime.UtcNow;
            newArticle.Author = author;

            _context.Articles.Add(newArticle);
            _context.ArticleTags.AddRange(articleTags);
            await _context.SaveChangesAsync(cancellationToken);

            return new ArticleDetailWrapper(newArticle);
        }

    }
}