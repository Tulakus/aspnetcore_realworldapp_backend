using FluentValidation;
using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
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