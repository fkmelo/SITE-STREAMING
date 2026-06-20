using System.ComponentModel.DataAnnotations;

namespace MeloStream.Models
{
    public class Episode
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 20, ErrorMessage = "La saison doit être comprise entre 1 et 20.")]
        public int Saison { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Le numéro d'épisode doit être supérieur à 0.")]
        public int Numero { get; set; }

        [StringLength(150, ErrorMessage = "Le titre de l'épisode est trop long.")]
        public string Titre { get; set; }

        // Clé étrangère pour lier l'épisode à son Anime
        public int AnimeId { get; set; }
        public Anime? Anime { get; set; }

        // Relation : Un épisode a plusieurs liens miroirs (Sibnet, etc.)
        public List<PlayerLink> PlayerLinks { get; set; } = new();
    }
}
