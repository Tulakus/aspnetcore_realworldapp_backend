using FluentValidation;
using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
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