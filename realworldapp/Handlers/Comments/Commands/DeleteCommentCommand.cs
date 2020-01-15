using FluentValidation;
using MediatR;
using realworldapp.Handlers.Comments.Responses;

namespace realworldapp.Handlers.Comments.Commands
{
    public class DeleteCommentCommand : IRequest<CommentWrapper>
    {
        public int CommentId { get; set; }
        public string Slug { get; set; }
    }

    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(c => c.Slug).NotEmpty();
            RuleFor(c => c.CommentId).GreaterThanOrEqualTo(0);
        }
    }
}