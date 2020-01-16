using realworldapp.Models;

namespace realworldapp.Handlers.Profiles.Response
{
    public class ProfileDto
    {
        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public bool Following { get; set; }

        public ProfileDto(Profile user)
        {
            Username = user.Username;
            Bio = user.Bio;
            Image = user.Image;
            Following = true;
        }
    }
}