using System.Collections.Generic;
using System.Linq;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Responses
{
    public class ArticleListWrapper
    {
        public IEnumerable<ArticleDetailDto> Articles { get; private set; }
        public int ArticlesCount { get; set; }

        public ArticleListWrapper(IList<Article> articles)
        {
            Articles = articles.Select(i => new ArticleDetailDto(i));
            ArticlesCount = articles.Count();
        }

    }
}