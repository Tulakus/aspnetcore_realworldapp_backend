using FluentValidation;
using MediatR;
using realworldapp.Handlers.Profiles.Response;

namespace realworldapp.Handlers.Profiles.Commands
{
    public class UnfollowUserCommand: IRequest<ProfileWrapper>
    {
        public string Username { get; set; }

        public UnfollowUserCommand(string username)
        {
            Username = username;
        }
    }

    public class UnfollowUserCommandValidator : AbstractValidator<FollowUserCommand>
    {
        public UnfollowUserCommandValidator()
        {
            RuleFor(c => c.Username).NotEmpty();
        }
    }
}