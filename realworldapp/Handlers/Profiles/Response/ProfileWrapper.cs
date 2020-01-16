using realworldapp.Models;

namespace realworldapp.Handlers.Profiles.Response
{
    public class ProfileWrapper
    {
        public ProfileDto Profile { get; set; }

        public ProfileWrapper(Profile user)
        {
            Profile = new ProfileDto(user);
        }
    }
}