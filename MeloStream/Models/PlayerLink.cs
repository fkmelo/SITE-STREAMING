namespace MeloStream.Models
{
    public class PlayerLink
    {
        public int Id { get; set; }
        public string NomPlateforme { get; set; } = string.Empty; // "Sibnet", "Sendvid"
        public string UrlEmbed { get; set; } = string.Empty;     // L'URL brute de l'iframe

        // Clé étrangère pour lier le lecteur à son Épisode
        public int EpisodeId { get; set; }
        public Episode? Episode { get; set; }
    }
}
