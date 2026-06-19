namespace MeloStream.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Saison { get; set; }
        public string Titre { get; set; }

        // Clé étrangère pour lier l'épisode à son Anime
        public int AnimeId { get; set; }
        public Anime? Anime { get; set; }

        // Relation : Un épisode a plusieurs liens miroirs (Sibnet, etc.)
        public List<PlayerLink> PlayerLinks { get; set; } = new();
    }
}
