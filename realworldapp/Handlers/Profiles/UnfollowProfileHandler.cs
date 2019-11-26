using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Profiles.Commands;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Profiles
{
    public class UnfollowProfileHandler: IRequestHandler<UnfollowUserCommand, ProfileWrapper>
    {
        private AppDbContext _context;

        public UnfollowProfileHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileWrapper> Handle(UnfollowUserCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Username))
                return null;

            var currentUserName = _context.UserInfo.Username;

            var usersQueryable = _context.Users.Include(i => i.Profile);

            var currentUser = await usersQueryable.FirstOrDefaultAsync(i => i.Username == currentUserName, cancellationToken);
            var unfollowedUser = await usersQueryable.FirstOrDefaultAsync(i => i.Username == command.Username, cancellationToken);

            if (currentUser == default || unfollowedUser == default)
                return null;

            var follower = _context.Followers.Include(i => i.Followed)
                .Include(i => i.Follower)
                .FirstOrDefault(i => i.Followed == unfollowedUser.Profile && i.Follower == currentUser.Profile);

            if (follower == default)
                return null;

            _context.Followers.Remove(follower);

            await _context.SaveChangesAsync(cancellationToken);
            return new ProfileWrapper(unfollowedUser.Profile);
        }
    }
}