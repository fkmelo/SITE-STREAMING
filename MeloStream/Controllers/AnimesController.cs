using Microsoft.AspNetCore.Mvc;
using MeloStream.Data;
using MeloStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MeloStream.Controllers
{
    public class AnimesController : Controller
    {
        private readonly AppDbContext _context;

        // On injecte ton AppDbContext ici
        public AnimesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Page qui affiche la liste des animes (https://localhost:XXXX/Animes)
        public async Task<IActionResult> Index()
        {
            var animes = await _context.Animes.ToListAsync();
            return View(animes);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // tous les épisodes liés à cet anime en une seule requête SQL !
            var anime = await _context.Animes
                .Include(a => a.Episodes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (anime == null)
            {
                return NotFound();
            }

            //trie les épisodes par saison puis par numéro pour l'affichage
            anime.Episodes = anime.Episodes.OrderBy(e => e.Saison).ThenBy(e => e.Numero).ToList();

            return View(anime);
        }

        // 2. Page qui affiche le formulaire de création (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. Action qui reçoit les données du formulaire et les sauvegarde (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Anime anime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anime);
                await _context.SaveChangesAsync(); // Sauvegarde physique dans ton fichier .db
                return RedirectToAction(nameof(Index)); // Redirige vers la liste
            }
            return View("Create",anime);
        }
    }
}