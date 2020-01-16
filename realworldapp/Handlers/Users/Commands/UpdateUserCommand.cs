using FluentValidation;
using MediatR;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Handlers.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserWrapper>
    {
        public UpdateData User { get; set; }
    }

    public class UpdateData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(r => r.User).NotNull();
        }
    }
}