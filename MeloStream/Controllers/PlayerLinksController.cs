using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MeloStream.Data;
using MeloStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MeloStream.Controllers
{
    public class PlayerLinksController : Controller
    {
        private readonly AppDbContext _context;

        public PlayerLinksController(AppDbContext context)
        {
            _context = context;
        }

        // URL : /admin/players
        [HttpGet("admin/players")]
        public async Task<IActionResult> Index()
        {
            var links = await _context.PlayerLinks
                .Include(p => p.Episode)
                .ThenInclude(e => e.Anime)
                .ToListAsync();
            return View(links);
        }

        // URL : /admin/players/ajouter (Affichage du formulaire)
        [HttpGet("admin/players/ajouter")]
        public async Task<IActionResult> Create()
        {
            var episodes = await _context.Episodes.Include(e => e.Anime).ToListAsync();
            ViewBag.EpisodeId = new SelectList(episodes.Select(e => new {
                Id = e.Id,
                Texte = $"{e.Anime?.Titre} - S{e.Saison} Ép{e.Numero}"
            }), "Id", "Texte");
            return View();
        }

        // URL : /admin/players/ajouter (Traitement du formulaire)
        [HttpPost("admin/players/ajouter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerLink playerLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var episodes = await _context.Episodes.Include(e => e.Anime).ToListAsync();
            ViewBag.EpisodeId = new SelectList(episodes.Select(e => new {
                Id = e.Id,
                Texte = $"{e.Anime?.Titre} - S{e.Saison} Ép{e.Numero}"
            }), "Id", "Texte", playerLink.EpisodeId);
            return View(playerLink);
        }
    }
}