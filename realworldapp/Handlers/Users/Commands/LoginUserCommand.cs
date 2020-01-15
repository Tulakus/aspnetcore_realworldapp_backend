using FluentValidation;
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

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(r => r.User).SetValidator(new LoginUserDataValidator());
        }
    }

    public class LoginUserDataValidator : AbstractValidator<LoginUserData>
    {
        public LoginUserDataValidator()
        {
            RuleFor(p => p.Email).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}
