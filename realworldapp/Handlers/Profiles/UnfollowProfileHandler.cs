using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Error;
using realworldapp.Handlers.Profiles.Commands;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Infrastructure;
using realworldapp.Models;

namespace realworldapp.Handlers.Profiles
{
    public class UnfollowProfileHandler: IRequestHandler<UnfollowUserCommand, ProfileWrapper>
    {
        private readonly AppDbContext _context;

        public UnfollowProfileHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileWrapper> Handle(UnfollowUserCommand command, CancellationToken cancellationToken)
        {
            var currentUserName = _context.UserInfo.Username;

            var usersQueryable = _context.Users.Include(i => i.Profile);

            var currentUser = await usersQueryable.FirstOrDefaultAsync(i => i.Profile.Username == currentUserName, cancellationToken);
            var unfollowedUser = await usersQueryable.FirstOrDefaultAsync(i => i.Profile.Username == command.Username, cancellationToken);

            if (currentUser == default)
                throw new NotFoundCommandException(new { User = $"{currentUserName} {ErrorMessages.NotFound}"});

            if (unfollowedUser == default)
                throw new NotFoundCommandException(new { User = $"{command.Username} {ErrorMessages.NotFound}" });

            var follower = _context.Followers.Include(i => i.Followed)
                .Include(i => i.Following)
                .FirstOrDefault(i => i.Followed.ProfileId == unfollowedUser.Profile.ProfileId && i.Following.ProfileId == currentUser.Profile.ProfileId);

            if (follower != default)
            {
                _context.Followers.Remove(follower);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new ProfileWrapper(unfollowedUser.Profile);
        }
    }
}