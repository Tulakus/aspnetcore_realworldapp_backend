namespace realworldapp.Models
{
    public class ArticleProfile
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int ProfileId { get; set; }
        public Profile Person { get; set; }
    }
}