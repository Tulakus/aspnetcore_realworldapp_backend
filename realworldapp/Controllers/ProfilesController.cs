using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using realworldapp.Handlers.Profiles.Commands;
using realworldapp.Handlers.Profiles.Response;

namespace realworldapp.Controllers
{
    [Route("api/[controller]/{username}")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfilesController( IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ProfileWrapper> GetUser([FromRoute] string username)
        {
            return await _mediator.Send(new QueryProfileCommand(username));
        }

  

        [HttpPost("follow")]
        public async Task<ProfileWrapper> FollowUser([FromRoute] string username)
        {
            return await _mediator.Send(new FollowUserCommand(username), new CancellationToken());
        }

        [HttpDelete("follow")]
        public async Task<ProfileWrapper> UnfollowUser([FromRoute] string username)
        {
            return await _mediator.Send(new UnfollowUserCommand(username), new CancellationToken());
        }
    }
}