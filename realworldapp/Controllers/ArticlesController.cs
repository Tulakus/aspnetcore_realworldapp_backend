using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using realworldapp.Handlers.Articles;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;
using System.Threading.Tasks;

namespace realworldapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ArticleListWrapper> GetArticles([FromQuery] QueryArticlesCommand command, [FromRoute] string slug)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{slug}")]
        public async Task<ArticleDetailWrapper> GetArticle([FromRoute] string slug)
        {
            return await _mediator.Send(new QueryArticleCommand(){Slug = slug});
        }

        [HttpGet("feed")]
        [Authorize]
        public async Task<ArticleListWrapper> GetFeedArticles()
        {
            return await _mediator.Send(new QueryArticlesFeedCommand());
        }

        // PUT: api/Articles/5
        [HttpPut("{slug}")]//todo zkontrolovat
        [Authorize]
        public async Task<ArticleDetailWrapper> UpdateArticle([FromRoute] string slug, [FromBody] UpdateArticleCommand command)
        {
            command.Article.Slug = slug;
            return await _mediator.Send(command);
        }

        // POST: api/Articles
        [HttpPost]
        [Authorize]
        public async Task<ArticleDetailWrapper> PostArticle([FromBody] CreateArticleCommand command)
        {
            var resp = await _mediator.Send(command);
            return resp;
        }

        // DELETE: api/Articles/5
        [HttpDelete("{slug}")]
        [Authorize]
        public async Task<Unit> DeleteArticle([FromRoute] string slug)
        {
            return await _mediator.Send(new DeleteArticleCommand(slug));
        }

        // DELETE: api/Articles/5
        [HttpPost("{slug}/favorite")]
        [Authorize]
        public async Task<ArticleDetailWrapper> FavoriteArticle([FromRoute] FavoriteArticleCommand command)
        {
            return await _mediator.Send(command);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{slug}/favorite")]
        [Authorize]
        public async Task<ArticleDetailWrapper> UnfavoriteArticle([FromRoute] UnfavoriteArticleCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}