using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using realworldapp;
using realworldapp.Handlers.Articles;
using realworldapp.Handlers.Articles.Commands;
using realworldapp.Handlers.Comments.Commands;
using realworldapp.Handlers.Users.Commands;
using realworldapp.Infrastructure.Security.Session;
using realworldapp.Models;

namespace RealWorldApp.IntegrationTests.Tests
{
    class Helper
    {
        public static async Task EnsureUserIsCreatedAndSetInDatabaseContext()
        {
            try
            {
                var command = new RegisterUserCommand()
                {
                    User =
                    new UserData(){
                        Email = "user1@test.com",
                        Password = "password",
                        Username = "user1"
                    }
                };
                await SliceFixture.SendAsync(command);

                var user = await SliceFixture.ExecuteDbContextAsync((ctx, mediator) =>

                    ctx.Users.FirstOrDefaultAsync(i => i.Login == command.User.Email) 
                );

                await SliceFixture.ExecuteDbContextAsync((ctx, mediator) =>
                {
                    ctx.UserInfo = new UserInfo()
                    {
                        ProfileId = user.UserId,
                        Username = command.User.Username,
                        Email = command.User.Email

                    };
                    return Task.CompletedTask;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
        }

        public static async Task CreateDefaultArticle()
        {
            var command = new CreateArticleCommand()
            {
                Article = new ArticleBase()
                {
                    Body = DefaultArticleConstants.Body,
                    Title = DefaultArticleConstants.Title,
                    Description = DefaultArticleConstants.Description,
                    TagList = DefaultArticleConstants.TagList
                }
            };

            await SliceFixture.ExecuteDbContextAsync(async (ctx, mediator) =>
            {
                await mediator.Send(command);
            });
        }

        public static async Task<Article> CreateDefaultArticle_ReturnDefaultArticleDto()
        {
            await CreateDefaultArticle();
            return await QueryDefaultArticleDto();
        }

        public static async Task<Article> QueryDefaultArticleDto()
        {
            return await SliceFixture.ExecuteDbContextAsync(db =>
                db.Articles.IncludeAllArticleInformationNotTracking()
                    .Include(i => i.Comments)
                    .SingleOrDefaultAsync());
        }

        public const string DefaultCommentBody = "Comment body";
        public static async Task CreateDefaultComment(string articleSlug)
        {
            var createComment = new CreateCommentCommand()
            {
                Slug = articleSlug,
                Comment = new CommentData()
                {
                    Body = DefaultCommentBody
                }
            };

            await SliceFixture.ExecuteDbContextAsync(async (ctx, mediator) =>
            {
                await mediator.Send(createComment);
            });
        }

    }
}
