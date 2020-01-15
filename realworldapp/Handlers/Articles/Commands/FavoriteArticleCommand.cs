using FluentValidation;
using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Commands
{
    public class FavoriteArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }
    }

    public class FavoriteArticleCommandValidator : AbstractValidator<FavoriteArticleCommand>
    {
        public FavoriteArticleCommandValidator()
        {
            RuleFor(c => c.Slug).NotEmpty();
        }
    }

}