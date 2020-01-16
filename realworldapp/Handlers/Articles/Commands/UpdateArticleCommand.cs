using MediatR;
using realworldapp.Handlers.Articles.Responses;

namespace realworldapp.Handlers.Articles.Commands
{
    public class UpdateArticleCommand : IRequest<ArticleDetailWrapper>
    {
        public UpdateArticleData Article { get; set; }
    }

    public class UpdateArticleData : ArticleBase
    {
        public string Slug { get; set; }
    }
}