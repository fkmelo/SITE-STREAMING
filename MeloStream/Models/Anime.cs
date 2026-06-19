namespace MeloStream.Models
{
    public class Anime
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        // Relation : Un anime a plusieurs épisodes
        public List<Episode> Episodes { get; set; } = new();
    }
}
