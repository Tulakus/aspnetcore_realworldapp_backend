using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
{
    public class FavoriteArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }
    }
}