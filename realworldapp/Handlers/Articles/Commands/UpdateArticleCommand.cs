using System.Collections.Generic;
using FluentValidation;
using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Commands
{
    public class UpdateArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public UpdateArticleData Article { get; set; }
    }

    public class UpdateArticleData : ArticleBase
    {
        public string Slug { get; set; }
    }

    public class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleCommandValidator()
        {
            RuleFor(c => c.Article).SetValidator(new UpdateArticleDataValidator());
        }
    }

    public class UpdateArticleDataValidator : ArticleBaseValidator<UpdateArticleData> 
    {
        public UpdateArticleDataValidator()
        {
            RuleFor(c => c.Slug).NotNull();
        }
    }
}