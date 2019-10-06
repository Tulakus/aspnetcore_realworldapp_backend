using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using realworldapp.Handlers.Users.Commands;
using realworldapp.Handlers.Users.Responses;
using realworldapp.Models;

namespace realworldapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public UsersController(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [Route("login")]
        [HttpPost]
        public async Task<UserWrapper> Login([FromBody] LoginUserCommand command)
        {
            return await _mediator.Send(command, new CancellationToken());

        }

        [HttpPost]
        public async Task<UserWrapper> Registration([FromBody] RegisterUserCommand command)
        {
            return await _mediator.Send(command, new CancellationToken());
        }

        [Route("~/api/user")]
        [HttpPut]
        [Authorize]
        public async Task<UserWrapper> ModifyItem([FromBody] UpdateUserCommand command)
        {
            return await _mediator.Send(command, new CancellationToken());
        }

        [Route("~/api/user")]
        [HttpGet]
        [Authorize]
        public async Task<UserWrapper> CurrentUser()
        {
            return await _mediator.Send(new QueryUserInfoCommand(), new CancellationToken());
        }
    }
}