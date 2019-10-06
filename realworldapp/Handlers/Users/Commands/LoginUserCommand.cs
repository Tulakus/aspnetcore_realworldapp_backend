using MediatR;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Handlers.Users.Commands
{
    public class LoginUserCommand: IRequest<UserWrapper>
    {
        public LoginUserData User { get; set; }
    }

    public class LoginUserData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
