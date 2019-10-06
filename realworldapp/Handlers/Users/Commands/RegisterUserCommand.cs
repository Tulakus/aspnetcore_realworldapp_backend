using MediatR;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Handlers.Users.Commands
{
    public class RegisterUserCommand: IRequest<UserWrapper>
    {
        public UserData User { get; set; }
    }

    public class UserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}