using System.Linq;
using Microsoft.AspNetCore.Http;

namespace realworldapp.Infrastructure.Security.CurrentUser
{
    public class CurrentUserAccessor: ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUser()
        {
            var usernameClaim = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(i => i.Type == "username");

            return usernameClaim?.Value;
        }
    }
}
