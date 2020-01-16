using FluentValidation;
using MediatR;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Handlers.Users.Commands
{
    public class RegisterUserCommand : IRequest<UserWrapper>
    {
        public UserData User { get; set; }
    }

    public class UserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(r => r.User).SetValidator(new UserDataValidator());
        }
    }

    public class UserDataValidator : AbstractValidator<UserData>
    {
        public UserDataValidator()
        {
            RuleFor(p => p.Username).NotEmpty();
            RuleFor(p => p.Email).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}