using MeloStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MeloStream.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // On indique à EF Core les tables à générer
        public DbSet<Anime> Animes { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<PlayerLink> PlayerLinks { get; set; }
    }
}