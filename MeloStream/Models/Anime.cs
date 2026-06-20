using System.ComponentModel.DataAnnotations;

namespace MeloStream.Models
{
    public class Anime
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères.")]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le synopsis est obligatoire.")]
        [StringLength(2000, ErrorMessage = "Le synopsis ne peut pas dépasser 2000 caractères.")]
        public string Synopsis { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'URL de l'image est obligatoire.")]
        [Url(ErrorMessage = "L'URL fournie n'est pas valide.")]
        public string ImageUrl { get; set; } = string.Empty;

        // Relation : Un anime a plusieurs épisodes
        public List<Episode> Episodes { get; set; } = new List<Episode>();
    }
}
