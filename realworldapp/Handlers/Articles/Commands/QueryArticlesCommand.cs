using MediatR;
using realworldapp.Handlers.Articles.Response;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles
{
    public class QueryArticlesCommand : IRequest<ArticleListWrapper>
    {
    }
}