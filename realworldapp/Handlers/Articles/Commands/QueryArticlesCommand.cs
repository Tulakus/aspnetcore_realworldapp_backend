using MediatR;
using realworldapp.Handlers.Articles.Response;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticlesCommand : IQueryPagination, IRequest<ArticleListWrapper>
    {
        public string Author { get; set; }
        public string Tag { get; set; }
        public string Favorited { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}