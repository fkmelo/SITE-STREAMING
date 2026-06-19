using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MeloStream.Data;
using MeloStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MeloStream.Controllers
{
    public class EpisodesController : Controller
    {
        private readonly AppDbContext _context;

        public EpisodesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var episodes = await _context.Episodes.Include(e => e.Anime).ToListAsync();
            return View(episodes);
        }

        public async Task<IActionResult> Watch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Chercher l'épisode, son Anime associé, ET tous les PlayerLinks (lecteurs) liés !
            var episode = await _context.Episodes
                .Include(e => e.Anime)
                .Include(e => e.PlayerLinks)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.AnimeId = new SelectList(await _context.Animes.ToListAsync(), "Id", "Titre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Episode episode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AnimeId = new SelectList(await _context.Animes.ToListAsync(), "Id", "Titre", episode.AnimeId);
            return View(episode);
        }
    }
}