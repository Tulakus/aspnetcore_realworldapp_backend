using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using realworldapp.Handlers.Articles;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;

namespace realworldapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public ArticlesController(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ArticleListWrapper> GetArticles()
        {
            return await _mediator.Send(new QueryArticlesCommand());
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles/5
        [HttpPut("{slug}")]
        public async Task<ArticleDetailWrapper> UpdateArticle([FromRoute] string slug, [FromBody] UpdateArticleCommand command)
        {
            return await _mediator.Send(command, new CancellationToken());
        }

        // POST: api/Articles
        [HttpPost]
        public async Task<ArticleDetailWrapper> PostArticle([FromBody]  CreateArticleCommand command)
        {
            var resp =  await _mediator.Send(command, new CancellationToken());
            return resp;
        }

        // DELETE: api/Articles/5
        [HttpDelete("{slug}")]
        public async Task<Unit> DeleteArticle([FromRoute] string slug)
        {
            return await _mediator.Send(new DeleteArticleCommand(slug));
        }

        private bool ArticleExists(string id)
        {
            return _context.Articles.Any(e => e.Slug == id);
        }
    }
}