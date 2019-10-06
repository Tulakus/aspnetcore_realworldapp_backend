using System.Collections.Generic;

namespace realworldapp.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<UserFollower> FollowedPeople { get; set; }
        public ICollection<UserFollower> Followers { get; set; }
    }
}