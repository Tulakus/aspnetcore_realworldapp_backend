using MediatR;
using realworldapp.Handlers.Profiles.Response;

namespace realworldapp.Handlers.Profiles.Commands
{
    public class QueryProfileCommand : IRequest<ProfileWrapper>
    {
        public string Username { get; set; }

        public QueryProfileCommand(string username)
        {
            Username = username;
        }
    }
}