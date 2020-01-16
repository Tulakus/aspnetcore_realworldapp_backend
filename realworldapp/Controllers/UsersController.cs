using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using realworldapp.Handlers.Users.Commands;
using realworldapp.Handlers.Users.Responses;

namespace realworldapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("login")]
        [HttpPost]
        public async Task<UserWrapper> Login([FromBody] LoginUserCommand command)
        {
            return await _mediator.Send(command);

        }

        [HttpPost]
        public async Task<UserWrapper> Registration([FromBody] RegisterUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [Route("~/api/user")]
        [HttpPut]
        [Authorize]
        public async Task<UserWrapper> ModifyItem([FromBody] UpdateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [Route("~/api/user")]
        [HttpGet]
        [Authorize]
        public async Task<UserWrapper> CurrentUser()
        {
            return await _mediator.Send(new QueryUserInfoCommand());
        }
    }
}