using FluentValidation;
using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Commands
{
    public class UnfavoriteArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }
    }

    public class UnfavoriteArticleCommandValidator : AbstractValidator<UnfavoriteArticleCommand>
    {
        public UnfavoriteArticleCommandValidator()
        {
            RuleFor(c => c.Slug).NotEmpty();
        }
    }
}