using MediatR;

namespace realworldapp.Handlers.Articles
{
    public class DeleteArticleCommand: IRequest
    {
        public string Slug { get; private set; }

        public DeleteArticleCommand(string slug)
        {
            Slug = slug;
        }
    }
}