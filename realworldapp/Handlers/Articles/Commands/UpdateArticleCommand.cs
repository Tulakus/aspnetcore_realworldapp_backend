using System.Collections.Generic;
using FluentValidation;
using MediatR;
using realworldapp.Models;

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