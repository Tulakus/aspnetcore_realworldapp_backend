using FluentValidation;
using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
{
    public class CreateArticleCommand: IRequest<ArticleDetailWrapper>
    {
        public ArticleBase Article { get; set; }
    }


    public class CreateArticleCommandCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandCommandValidator()
        {
            RuleFor(c => c.Article).SetValidator(new ArticleBaseValidator<ArticleBase>());
        }
    }
}