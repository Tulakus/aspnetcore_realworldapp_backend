using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }
    }
}