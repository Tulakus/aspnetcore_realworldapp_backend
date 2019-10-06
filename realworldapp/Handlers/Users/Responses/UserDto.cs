using realworldapp.Models;

namespace realworldapp.Handlers.Users.Responses
{
    public class UserDto
    {
        public UserDto(User user)
        {
            Name = user.Username;
            Email = user.Email;
            Image = user.Profile.Image;
            Bio = user.Profile.Bio;
            Token = user.Token;
        }

        public string Slug { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
    }
}