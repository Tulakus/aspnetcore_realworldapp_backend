using System.Collections.Generic;
using System.Linq;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Response
{
    public class ArticleListWrapper
    {
        public IEnumerable<ArticleDetailDto> Articles { get; private set; }

        public ArticleListWrapper(IEnumerable<Article> articles)
        {
            Articles = articles.Select(i => new ArticleDetailDto(i));
        }

    }
}