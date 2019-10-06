using realworldapp.Models;

namespace realworldapp.Handlers.Users.Responses
{
    public class UserWrapper
    {
        public UserWrapper(User user)
        {
            User = new UserDto(user);
        }
        public UserDto User { get; set; }
    }
}