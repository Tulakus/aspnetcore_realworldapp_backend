using realworldapp.Models;

namespace realworldapp.Handlers.Tags.Response
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
            Following = true; //todo doresit
        }
    }
}