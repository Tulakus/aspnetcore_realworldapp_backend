using System.Collections.Generic;

namespace realworldapp.Handlers.Articles
{
    public class ArticleBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public IList<string> TagList { get; set; }
    }
}