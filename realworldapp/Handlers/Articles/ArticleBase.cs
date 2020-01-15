using System.Collections.Generic;
using FluentValidation;

namespace realworldapp.Handlers.Articles
{
    public class ArticleBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public IList<string> TagList { get; set; }
    }

    public class ArticleBaseValidator<T> : AbstractValidator<T>  where T: ArticleBase
    {
        public ArticleBaseValidator()
        {
            RuleFor(c => c.Title).NotNull();
            RuleFor(c => c.Body).NotNull();
        }
    }
}