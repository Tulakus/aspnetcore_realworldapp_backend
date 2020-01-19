using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using realworldapp.Handlers.Comments.Commands;

namespace RealWorldApp.IntegrationTests.Tests.Comments
{
    [TestFixture]
    class DeleteCommentHandlerTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteArticleComment()
        {
            await Helper.EnsureUserIsCreatedAndSetInDatabaseContext();
            var articleDto = await Helper.CreateDefaultArticle_ReturnDefaultArticleDto();

            await Helper.CreateDefaultComment(articleDto.Slug);
            var originalArticleWithComment = await Helper.QueryDefaultArticleDto();

            Assert.AreEqual(1, originalArticleWithComment.Comments.Count);

            var commentId = originalArticleWithComment.Comments.First().CommentId;

            var deleteCommand = new DeleteCommentCommand {Slug = originalArticleWithComment.Slug, CommentId = commentId };
            await SliceFixture.ExecuteDbContextAsync(async (ctx, mediator) =>
            {
                await mediator.Send(deleteCommand);
            });

            var resultArticleWithComment = await Helper.QueryDefaultArticleDto();
            var resultCommentsCount = resultArticleWithComment.Comments.Count;

            Assert.AreEqual(0, resultCommentsCount);
        }
    }
}
