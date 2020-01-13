using MediatR;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticlesFeedCommand : IQueryPagination, IRequest<ArticleListWrapper>
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}