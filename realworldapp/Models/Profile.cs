using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using realworldapp.Infrastructure.Security.Session;

namespace realworldapp.Models
{
    public class Profile
    {
        [ForeignKey(nameof(User))]
        public int ProfileId { get; set; }
        public string Email { get; set; }
        public string Slug { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
   
        public User User { get; set; }
        public ICollection<UserFollower> Followers { get; set; }
        public ICollection<UserFollower> Following { get; set; }
        public ICollection<ArticleProfile> FavoritedArticles { get; set; }
    }
}