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
    public class FollowProfileHandler: IRequestHandler<FollowUserCommand, ProfileWrapper>
    {
        private AppDbContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public FollowProfileHandler(AppDbContext context, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }


        public async Task<ProfileWrapper> Handle(FollowUserCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Username))
                return null;

            var currentUserName = _currentUserAccessor.GetCurrentUser();

            var currentUser = await _context.Users.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Username == currentUserName, cancellationToken);
            var followedUser = await _context.Users.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Username == command.Username, cancellationToken);

            if (currentUser == default || followedUser == default)
                return null;

            await _context.Followers.AddAsync(new UserFollower
            {
                Followed = followedUser.Profile,
                Follower = currentUser.Profile
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return new ProfileWrapper(followedUser.Profile);
        }
    }
}