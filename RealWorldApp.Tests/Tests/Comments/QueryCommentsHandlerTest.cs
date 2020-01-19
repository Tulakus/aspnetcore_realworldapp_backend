using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using realworldapp.Handlers.Comments.Commands;

namespace RealWorldApp.IntegrationTests.Tests.Comments
{
    [TestFixture]
    class QueryCommentsHandlerTest : TestBase
    {
        [Test]
        public async Task ShouldReturnArticleComments()
        {
            await Helper.EnsureUserIsCreatedAndSetInDatabaseContext();

            var article = await Helper.CreateDefaultArticle_ReturnDefaultArticleDto();
            await Helper.CreateDefaultComment(article.Slug);
            var queryCommentsCommand = new QueryCommentsCommand(article.Slug);

            var commandResult = await SliceFixture.SendAsync(queryCommentsCommand);
            var comments = commandResult.Comments;

            Assert.NotNull(comments);
            Assert.GreaterOrEqual(1, comments.Count);
            Assert.Contains(Helper.DefaultCommentBody, comments.Select(i => i.Body).ToList());
        }

    }
}
