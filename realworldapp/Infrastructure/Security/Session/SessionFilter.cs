using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using realworldapp.Infrastructure.Security.JWT;
using realworldapp.Models;

namespace realworldapp.Infrastructure.Security.Session
{
    public class SessionFilter: IAsyncActionFilter
    {
        private readonly AppDbContext _dbContext;

        public SessionFilter(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userIdentity = (ClaimsIdentity)context.HttpContext.User.Identity;

            if (userIdentity.IsAuthenticated)
            {
                _dbContext.UserInfo = GetUserInfo(userIdentity.Claims);
            }

            var result = await next();
        }

        UserInfo GetUserInfo(IEnumerable<Claim> claims)
        {
            if (claims == null)
                return null;

            var claimList = claims.ToList();

            var userName = claimList.FirstOrDefault(i => i.Type == CustomClaims.UserName)?.Value;
            var email = claimList.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value;
            var userId = claimList.FirstOrDefault(i => i.Type == CustomClaims.UserId)?.Value;

            return new UserInfo
            {
                Username = userName,
                Email = email,
                // ReSharper disable once AssignNullToNotNullAttribute
                ProfileId = int.Parse(userId)
            };
        }
    }
}
