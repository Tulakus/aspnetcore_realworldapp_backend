using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Profiles.Commands;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Profiles
{
    public class FollowProfileHandler: IRequestHandler<FollowUserCommand, ProfileWrapper>
    {
        private AppDbContext _context;

        public FollowProfileHandler(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ProfileWrapper> Handle(FollowUserCommand command, CancellationToken cancellationToken)
        {
            var currentUserName = _context.UserInfo.Username;
            var usersQueryable = _context.Users.Include(i => i.Profile);

            var currentUser = await usersQueryable.FirstOrDefaultAsync(i => i.Profile.Username == currentUserName, cancellationToken);
            var followedUser = await usersQueryable.FirstOrDefaultAsync(i => i.Profile.Username == command.Username, cancellationToken);

            if (currentUser == default || followedUser == default)
                return null;

            await _context.Followers.AddAsync(new UserFollower
            {
                Followed = followedUser.Profile,
                Following = currentUser.Profile
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return new ProfileWrapper(followedUser.Profile);
        }
    }
}