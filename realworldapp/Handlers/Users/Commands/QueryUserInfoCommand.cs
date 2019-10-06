using MediatR;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Handlers.Users.Commands
{
    public class QueryUserInfoCommand: IRequest<UserWrapper>
    {
    }
}
