using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Profiles.Commands;
using realworldapp.Handlers.Profiles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Profiles
{
    public class QueryProfileHandler: IRequestHandler<QueryProfileCommand, ProfileWrapper>
    {
        private AppDbContext _context;

        public QueryProfileHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileWrapper> Handle(QueryProfileCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Username))
                return null;

            var profile = await _context.Profiles.Include(i => i.User).FirstOrDefaultAsync(i => i.Username == command.Username, cancellationToken);

            if (profile == default(Profile))
                return null;

            return new ProfileWrapper(profile);
        }
    }
}