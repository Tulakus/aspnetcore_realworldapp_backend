using FluentValidation;
using MediatR;
using realworldapp.Handlers.Comments.Responses;

namespace realworldapp.Handlers.Comments.Commands
{
    public class CreateCommentCommand : IRequest<CommentWrapper>
    {
        public CommentData Comment { get; set; }
        public string Slug { get; set; }
    }

    public class CommentData
    {
        public string Body { get; set; }
    }

    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(c => c.Slug).NotEmpty();
            RuleFor(c => c.Comment).NotNull().SetValidator(new CommentDataCommandValidator());
        }
    }

    public class CommentDataCommandValidator : AbstractValidator<CommentData>
    {
        public CommentDataCommandValidator()
        {
            RuleFor(c => c.Body).NotEmpty();
        }
    }
}