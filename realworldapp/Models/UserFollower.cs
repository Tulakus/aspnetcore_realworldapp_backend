namespace realworldapp.Models
{
    public class UserFollower
    {
        public int FollowedId { get; set; }
        public Profile Followed { get; set; }
        public int FollowingId { get; set; }
        public Profile Following { get; set; }
    }
}