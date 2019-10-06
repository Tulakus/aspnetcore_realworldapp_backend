using System.Linq;

namespace realworldapp.Models
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