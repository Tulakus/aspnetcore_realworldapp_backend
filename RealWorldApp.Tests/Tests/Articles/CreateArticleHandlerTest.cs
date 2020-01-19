using System.Threading.Tasks;
using NUnit.Framework;

namespace RealWorldApp.IntegrationTests.Tests.Articles
{
    [TestFixture]
    class CreateArticleHandlerTest : TestBase
    {
        [Test]
        public async Task ShouldCreateNewArticle()
        {
            await Helper.EnsureUserIsCreatedAndSetInDatabaseContext();
            var article = await Helper.CreateDefaultArticle_ReturnDefaultArticleDto();

            Assert.IsNotNull(article);
            Assert.AreEqual(DefaultArticleConstants.Title, article.Title);
            Assert.AreEqual(DefaultArticleConstants.Body, article.Body);
            Assert.AreEqual(DefaultArticleConstants.TagList.Count, article.ArticleTags.Count);
        }

    }
}
