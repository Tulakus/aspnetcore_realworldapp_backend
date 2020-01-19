using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using realworldapp.Handlers.Articles.Commands;

namespace RealWorldApp.IntegrationTests.Tests.Articles
{
    [TestFixture]
    class UpdateArticleHandlerTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateArticle()
        {
            await Helper.EnsureUserIsCreatedAndSetInDatabaseContext();
            var originalArticle = await Helper.CreateDefaultArticle_ReturnDefaultArticleDto();
            
            Assert.AreEqual(2, originalArticle.ArticleTags.Count);

            var updatedTitle = originalArticle + " edited";
            var updatedTags = originalArticle.ArticleTags.Select(i => i.Tag.Name).ToList();
            updatedTags.Add("tag3");
            
            var updateCommand = new UpdateArticleCommand()
            {
                Article = new UpdateArticleData()
                {
                    Slug = originalArticle.Slug,
                    Title = updatedTitle,
                    TagList = updatedTags
                }
            };

            await SliceFixture.ExecuteDbContextAsync(async (ctx, mediator) =>
            {
                await mediator.Send(updateCommand);
            });

            var updatedArticle = await Helper.QueryDefaultArticleDto();

            Assert.AreEqual(updatedTitle, updatedArticle.Title);
            Assert.AreEqual(3, updatedArticle.ArticleTags.Count);
        }

    }
}
