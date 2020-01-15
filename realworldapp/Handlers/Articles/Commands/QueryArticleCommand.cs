using FluentValidation;
using MediatR;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }
    }

    public class QueryArticleCommandValidator : AbstractValidator<QueryArticleCommand>
    {
        public QueryArticleCommandValidator()
        {
            RuleFor(c => c.Slug).NotEmpty();
        }
    }
}