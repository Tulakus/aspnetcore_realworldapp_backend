using realworldapp.Models;

namespace realworldapp.Handlers.Users.Responses
{
    public class UserDto
    {
        public UserDto(Profile userProfile, string token)
        {
            Name = userProfile.Username;
            Email = userProfile.Email;
            Image = userProfile.Image;
            Bio = userProfile.Bio;
            Slug = userProfile.Slug;
            Token = token;
            Username = userProfile.Username;
        }

        public string Slug { get; set; }
        public string Username { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
    }
}