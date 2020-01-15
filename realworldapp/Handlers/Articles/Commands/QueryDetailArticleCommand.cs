using FluentValidation;
using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryDetailArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }

        public QueryDetailArticleCommand(string slug)
        {
            Slug = slug;
        }
    }

    public class QueryDetailArticleCommandValidator : AbstractValidator<QueryDetailArticleCommand>
    {
        public QueryDetailArticleCommandValidator()
        {
            RuleFor(c => c.Slug).NotEmpty();
        }
    }
}