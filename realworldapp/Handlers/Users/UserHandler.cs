using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Users.Commands;
using realworldapp.Handlers.Users.Responses;
using realworldapp.Infrastructure.Security;
using realworldapp.Infrastructure.Security.JWT;
using realworldapp.Models;

namespace realworldapp.Handlers.Users
{
    public class UserHandler : IRequestHandler<RegisterUserCommand, UserWrapper>, IRequestHandler<LoginUserCommand, UserWrapper>,
        IRequestHandler<UpdateUserCommand, UserWrapper>, IRequestHandler<QueryUserInfoCommand, UserWrapper>
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHashProvider _passwordHashProvider;
        private readonly IJwt _jwtService;

        public UserHandler(AppDbContext context, IPasswordHashProvider passwordHashProvider, IJwt jwtService)
        {
            _context = context;
            _passwordHashProvider = passwordHashProvider;
            _jwtService = jwtService;
        }

        public async Task<UserWrapper> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var userData = command.User;
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Email == userData.Email, cancellationToken);
            if (user != default(User))
                return null; // todo - return already registered

            user = new User
            {
                Username = userData.Username,
                Email = userData.Email,
                Password = _passwordHashProvider.HashPassword(userData.Password),
                Profile = new Profile(),
            };

            var claims = GetUserClaims(user);
            user.Token = _jwtService.GenerateToken(claims);
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserWrapper(user);
        }

        private Claim[] GetUserClaims(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email), new Claim(CustomClaims.UserName, user.Username),
                new Claim(CustomClaims.UserId, user.UserId.ToString())
            };
            return claims;
        }

        public async Task<UserWrapper> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var commandUser = command.User;
            var user = await _context.Users.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Email == commandUser.Email, cancellationToken);
            if (user is default(User))
                return null; // todo - return already registered

            if (!_passwordHashProvider.VerifyPassword(command.User.Password, user.Password))
                return null;

            var claims = GetUserClaims(user);
            user.Token = _jwtService.GenerateToken(claims);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserWrapper(user);
        }


        public async Task<UserWrapper> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var commandUser = command.User;
            var currentUser = _context.UserInfo.Username;

            if (string.IsNullOrWhiteSpace(currentUser))
                throw new ArgumentException("Invalid user");

            var user = await _context.Users.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Username == currentUser, cancellationToken);

            if (user == default)
                return null; // todo - return not found

            user.Username = commandUser.Username ?? user.Username;
            user.Email = commandUser.Email ?? user.Email;
            user.Profile.Bio = commandUser.Bio ?? user.Profile.Bio;
            user.Profile.Image = commandUser.Image ?? user.Profile.Image;

            if (!string.IsNullOrWhiteSpace(commandUser.Password))
            {
                user.Password = _passwordHashProvider.HashPassword(commandUser.Password);
            }

            var claims = GetUserClaims(user);
            user.Token = _jwtService.GenerateToken(claims);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserWrapper(user);
        }

        public async Task<UserWrapper> Handle(QueryUserInfoCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _context.UserInfo.Username;

            if (string.IsNullOrWhiteSpace(currentUser))
                throw new ArgumentException("Invalid user");

            var user = await _context.Users.AsNoTracking().Include(i => i.Profile)
                .FirstOrDefaultAsync(i => i.Username == currentUser, cancellationToken);

            return new UserWrapper(user);
        }
    }
}