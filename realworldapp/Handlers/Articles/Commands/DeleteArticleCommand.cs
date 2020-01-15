using FluentValidation;
using MediatR;
using realworldapp.Handlers.Comments.Commands;

namespace realworldapp.Handlers.Articles
{
    public class DeleteArticleCommand: IRequest
    {
        public string Slug { get; private set; }

        public DeleteArticleCommand(string slug)
        {
            Slug = slug;
        }

        public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
        {
            public DeleteArticleCommandValidator()
            {
                RuleFor(c => c.Slug).NotEmpty();
            }
        }
    }
}