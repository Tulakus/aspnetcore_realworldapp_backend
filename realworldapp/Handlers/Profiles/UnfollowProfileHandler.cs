using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Profiles.Commands;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Infrastructure.Security.CurrentUser;
using realworldapp.Models;

namespace realworldapp.Handlers.Profiles
{
    public class UnfollowProfileHandler: IRequestHandler<UnfollowUserCommand, ProfileWrapper>
    {
        private AppDbContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UnfollowProfileHandler(AppDbContext context, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ProfileWrapper> Handle(UnfollowUserCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Username))
                return null;

            var currentUserName = _currentUserAccessor.GetCurrentUser();

            var currentUser = await _context.Users.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Username == currentUserName, cancellationToken);
            var unfollowedUser = await _context.Users.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Username == command.Username, cancellationToken);

            if (currentUser == default || unfollowedUser == default)
                return null;
            var entities = _context.Followers.Include(i => i.Followed)
                .Include(i => i.Follower).FirstOrDefault(i => i.Followed == unfollowedUser.Profile && i.Follower == currentUser.Profile);

            if (entities == default)
                return null;

            _context.Followers.Remove(entities);

            await _context.SaveChangesAsync(cancellationToken);
            return new ProfileWrapper(unfollowedUser.Profile);
        }
    }
}