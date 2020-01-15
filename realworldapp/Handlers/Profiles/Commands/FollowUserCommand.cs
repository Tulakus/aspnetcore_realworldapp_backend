using FluentValidation;
using MediatR;
using realworldapp.Handlers.Profiles.Response;

namespace realworldapp.Handlers.Profiles.Commands
{
    public class FollowUserCommand : IRequest<ProfileWrapper>
    {
        public string Username { get; set; }

        public FollowUserCommand(string username)
        {
            Username = username;
        }
    }

    public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
    {
        public FollowUserCommandValidator()
        {
        }
    }
}