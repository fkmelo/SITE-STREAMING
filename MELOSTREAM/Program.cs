using Microsoft.EntityFrameworkCore;
using MeloStream.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// 1. SERVICES : Configuration des dépendances de l'application
// -------------------------------------------------------------------------

// [ESSENTIEL] Configuration de la vraie base de données SQLite
// Va lire la chaîne de connexion dans appsettings.json ou utilise "MeloStream.db" par défaut
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=StreamingDatabase.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Activation des fonctionnalités MVC et Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Gestion de la Session et des Cookies (Conservation de tes paramètres)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true; // [ESSENTIEL] Sécurise le cookie contre les scripts malveillants
    options.Cookie.IsEssential = true; // Requis pour que la session fonctionne même sans consentement RGPD
});

var app = builder.Build();

// -------------------------------------------------------------------------
// 2. MIDDLEWARES : Configuration du pipeline d'exécution HTTP
// -------------------------------------------------------------------------

// Gestion des environnements (Dev vs Production)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Ton astuce ultime : Désactivation du cache des fichiers statiques (CSS/JS/Images) en Dev
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = context => context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store")
    });
}
else
{
    // [ESSENTIEL] Gestion propre des pages d'erreurs (ex: 404, 500) en mode connecté/production
    app.UseExceptionHandler("/Home/Error");
    // [ESSENTIEL] Force l'utilisation du protocole HTTPS sécurisé
    app.UseHsts();

    app.UseStaticFiles();
}

app.UseHttpsRedirection(); // Redirige automatiquement le HTTP vers le HTTPS

app.UseRouting();

// [ESSENTIEL] L'ordre est super important : La Session doit TOUJOURS être activée AVANT l'autorisation
app.UseSession();
app.UseAuthorization();

// Configuration des routes de l'application (Version moderne et nettoyée)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Animes}/{action=Index}/{id?}"); // Catalogue configuré en page d'accueil par défaut

app.MapRazorPages();

// Lancement de l'application
app.Run();