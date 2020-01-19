using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RealWorldApp.IntegrationTests.Tests.Comments
{
    [TestFixture]
    class CreateCommentHandlerTest: TestBase
    {
        [Test]
        public async Task ShouldCreateCommentToArticle()
        {
            await Helper.EnsureUserIsCreatedAndSetInDatabaseContext();
            var articleDto = await Helper.CreateDefaultArticle_ReturnDefaultArticleDto();

            await Helper.CreateDefaultComment(articleDto.Slug);
            var articleWithComment = await Helper.QueryDefaultArticleDto();

            Assert.IsNotNull(articleWithComment);
            Assert.AreEqual(1, articleWithComment.Comments.Count);
            Assert.AreEqual(Helper.DefaultCommentBody, articleWithComment.Comments.First().Body);
        }

    }
}
