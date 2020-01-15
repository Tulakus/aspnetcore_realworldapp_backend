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
    public class UpdateArticleHandler : IRequestHandler<UpdateArticleCommand, ArticleDetailWrapper>
    {
        private readonly AppDbContext _context;

        public UpdateArticleHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ArticleDetailWrapper> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
        {
            var changeCommand = command.Article;
            var articleTagList = changeCommand.TagList?.Distinct().ToList();

            var articleQueryable = _context.Articles
                .Include(i => i.ArticleTags)
                .ThenInclude(i => i.Tag)
                .Include(i => i.Author);

            var article = await articleQueryable.FirstOrDefaultAsync(i => i.Slug == changeCommand.Slug, cancellationToken);

            if (article == default(Article))
            {
                return null; // todo add invalid/notfound result
            }

            if (article.Author.ProfileId != _context.UserInfo.ProfileId)
            {
                return null; // todo return unauthorized
            }

            if (articleTagList == null || !articleTagList.Any())
            {
                if (article.ArticleTags.Any())
                {
                    _context.ArticleTags.RemoveRange(article.ArticleTags);
                }
            }
            else
            {
                var list = article.ArticleTags.Select(i => i.Tag.Name).ToList();
                var areEqual = list.All(articleTagList.Contains) && list.Count == articleTagList.Count;
                if (!areEqual)
                {
                    var removed = list.Except(articleTagList).ToList();
                    var added = articleTagList.Except(list).ToList();
                    if (added.Any())
                    {
                        await HandleAddedTags(added, article);
                    }

                    if (removed.Any())
                    {
                        await HandleRemovedTags(removed, article);
                    }
                }
            }

            if (!string.IsNullOrEmpty(changeCommand.Title))
                article.Title = changeCommand.Title;

            if (!string.IsNullOrEmpty(changeCommand.Description))
                article.Description = changeCommand.Description;

            if (!string.IsNullOrEmpty(changeCommand.Body))
                article.Body = changeCommand.Body;

            article.UpdatedAt = DateTime.UtcNow;

            _context.Update(article);
            await _context.SaveChangesAsync(cancellationToken);

            return new ArticleDetailWrapper(article);
        }

        private async Task<bool> HandleRemovedTags(List<string> removed, Article article)
        {
            foreach (var articleTag in removed)
            {
                var tag = await _context.ArticleTags.FirstOrDefaultAsync(i => i.Tag.Name == articleTag && i.Article.ArticleId == article.ArticleId);
                if (tag == default(ArticleTag))
                {
                    continue;
                }

                _context.ArticleTags.Remove(tag);
            }

            return true;
        }

        private async Task<bool> HandleAddedTags(List<string> added, Article article)
        {
            foreach (var articleTag in added)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(i => i.Name == articleTag);
                if (tag == default(Tag))
                {
                    tag = new Tag
                    {
                        Name = articleTag
                    };

                    _context.Tags.Add(tag);
                    _context.SaveChanges();
                }

                _context.ArticleTags.Add(new ArticleTag
                {
                    Article = article,
                    Tag = tag
                });
            }

            return true;
        }
    }
}