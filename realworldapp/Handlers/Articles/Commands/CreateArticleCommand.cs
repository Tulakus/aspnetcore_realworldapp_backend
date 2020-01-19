using FluentValidation;
using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
{
    public class CreateArticleCommand: IRequest<ArticleDetailWrapper>
    {
        public ArticleBase Article { get; set; }
    }


    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(c => c.Article).SetValidator(new ArticleBaseValidator<ArticleBase>());
        }
    }
}