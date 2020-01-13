using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Infrastructure.Security;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class CreateArticleHandler : IRequestHandler<CreateArticleCommand, ArticleDetailWrapper>
    {
        private AppDbContext _context;
        private ISlugGenerator _slugGenerator;

        public CreateArticleHandler(AppDbContext context, ISlugGenerator slugGenerator)
        {
            _context = context;
            _slugGenerator = slugGenerator;
        }
        public async Task<ArticleDetailWrapper> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
        {
            var userProfile = await _context.Profiles.GetProfile(_context.UserInfo, cancellationToken);
            var articleData = command.Article;
            var articleTagList = articleData.TagList;
            var article = new Article();
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
                        Article = article,
                        Tag = tag
                    });
                }
            }
            
            article.Title = articleData.Title;
            article.Description = articleData.Description;
            article.Body = articleData.Body;
            article.CreatedAt = DateTime.UtcNow;
            article.UpdatedAt = DateTime.UtcNow;
            article.Author = userProfile;
            article.Slug = _slugGenerator.Generate();

            _context.Articles.Add(article);
            _context.ArticleTags.AddRange(articleTags);
            await _context.SaveChangesAsync(cancellationToken);

            return new ArticleDetailWrapper(article);
        }
    }
}