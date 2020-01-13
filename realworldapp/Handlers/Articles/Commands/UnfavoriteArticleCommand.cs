using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Commands
{
    public class UnfavoriteArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }
    }

}