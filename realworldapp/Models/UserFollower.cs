using System.ComponentModel.DataAnnotations.Schema;

namespace realworldapp.Models
{
    public class UserFollower
    {
        public int FollowedId { get; set; }
        public Profile Followed { get; set; }
        public int FollowerId { get; set; }
        public Profile Follower { get; set; }
    }
}