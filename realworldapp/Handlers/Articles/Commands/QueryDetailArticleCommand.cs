using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryDetailArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public string Slug { get; set; }

        public QueryDetailArticleCommand(string slug)
        {
            Slug = slug;
        }
    }
}