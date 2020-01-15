using System.Collections.Generic;
using FluentValidation;
using MediatR;
using realworldapp.Models;

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