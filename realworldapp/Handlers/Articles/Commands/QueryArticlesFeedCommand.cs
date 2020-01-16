using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
{
    public class QueryArticlesFeedCommand : IQueryPagination, IRequest<ArticleListWrapper>
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}