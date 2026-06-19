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

        public async Task<IActionResult> Index()
        {
            var links = await _context.PlayerLinks
                .Include(p => p.Episode)
                .ThenInclude(e => e.Anime)
                .ToListAsync();
            return View(links);
        }

        public async Task<IActionResult> Create()
        {
            var episodes = await _context.Episodes.Include(e => e.Anime).ToListAsync();
            ViewBag.EpisodeId = new SelectList(episodes.Select(e => new {
                Id = e.Id,
                Texte = $"{e.Anime?.Titre} - S{e.Saison} Ép{e.Numero}"
            }), "Id", "Texte");
            return View();
        }

        [HttpPost]
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