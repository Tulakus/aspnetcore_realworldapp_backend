using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using realworldapp.Handlers.Articles.Commands;

namespace RealWorldApp.IntegrationTests.Tests.Articles
{
    [TestFixture]
    class DeleteArticleHandlerTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteArticle()
        {
            await Helper.EnsureUserIsCreatedAndSetInDatabaseContext();
            var articleDto = await Helper.CreateDefaultArticle_ReturnDefaultArticleDto();

            var originalArticleCount = await SliceFixture.ExecuteDbContextAsync(db => db.Articles.CountAsync());
            Assert.AreEqual(1, originalArticleCount);

            var deleteCommand = new DeleteArticleCommand(articleDto.Slug);

            await SliceFixture.ExecuteDbContextAsync(async (ctx, mediator) =>
            {
                await mediator.Send(deleteCommand);
            });

            var actualArticleCount = await SliceFixture.ExecuteDbContextAsync(db => db.Articles.CountAsync());
            Assert.AreEqual(0, actualArticleCount);
        }

    }
}
