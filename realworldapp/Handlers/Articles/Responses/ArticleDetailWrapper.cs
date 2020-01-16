using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Responses
{
    public class ArticleDetailWrapper
    {
        public ArticleDetailDto Article { get; }


        public ArticleDetailWrapper(Article article)
        {
            Article = new ArticleDetailDto(article);
        }

    }
}