using MediatR;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Handlers.Tags.Response;

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
}