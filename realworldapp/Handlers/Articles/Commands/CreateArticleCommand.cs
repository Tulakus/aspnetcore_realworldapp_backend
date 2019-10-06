using System.Collections.Generic;
using MediatR;
using realworldapp.Models;

namespace realworldapp.Handlers.Articles.Commands
{
    public class CreateArticleCommand: IRequest<ArticleDetailWrapper>
    {
        public CreateArticleData Article { get; set; }
    }

    public class CreateArticleData : ArticleBase
    {
    }
}