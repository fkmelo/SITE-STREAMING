using Microsoft.AspNetCore.Mvc;
using MeloStream.Data;
using MeloStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MeloStream.Controllers
{
    public class AnimesController : Controller
    {
        private readonly AppDbContext _context;

        public AnimesController(AppDbContext context)
        {
            _context = context;
        }

        // URL : /catalogue
        [HttpGet("catalogue")]
        public async Task<IActionResult> Index()
        {
            var animes = await _context.Animes.ToListAsync();
            return View(animes);
        }

        // URL : /anime/5
        [HttpGet("anime/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .Include(a => a.Episodes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (anime == null)
            {
                return NotFound();
            }

            anime.Episodes = anime.Episodes.OrderBy(e => e.Saison).ThenBy(e => e.Numero).ToList();

            return View(anime);
        }

        // URL : /admin/anime/ajouter (Affichage du formulaire)
        [HttpGet("admin/anime/ajouter")]
        public IActionResult Create()
        {
            return View();
        }

        // URL : /admin/anime/ajouter (Traitement du formulaire)
        [HttpPost("admin/anime/ajouter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Anime anime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", anime);
        }

        // 1. GET: Admin/Animes/Delete/5
        // Affiche la page de confirmation de suppression
        [HttpGet]
        [Route("Admin/Anime/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }

        // 2. POST: Admin/Animes/Delete/5
        // Traite la suppression réelle après confirmation
        [HttpPost, ActionName("Delete")]
        [Route("Admin/Anime/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime != null)
            {
                _context.Animes.Remove(anime);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}