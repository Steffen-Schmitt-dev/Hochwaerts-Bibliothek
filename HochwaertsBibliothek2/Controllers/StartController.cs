using HochwaertsBibliothek2.Data;
using HochwaertsBibliothek2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HochwaertsBibliothek2.Controllers;

public class StartController : Controller
{
    private readonly BibliothekDbContext _db;

    public StartController(BibliothekDbContext db)
        => _db = db;

    public async Task<IActionResult> Index()
    {
        var verkaufbar = await _db.Exemplare
            .Include(e => e.Buch)
            .Where(e => e.Status == VerleihStatus.Verkaufbar)
            .OrderBy(e => e.Buch!.Titel)
            .ToListAsync();

        return View(verkaufbar);
    }

    [HttpPost]
    public IActionResult SwitchTheme()
    {
        var currentTheme = Request.Cookies["theme"] ?? "theme-light";

        var newTheme = currentTheme == "theme-light"
            ? "theme-dark"
            : "theme-light";

        Response.Cookies.Append("theme", newTheme, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1)
        });

        var referer = Request.Headers["Referer"].ToString();

        if (string.IsNullOrWhiteSpace(referer))
            return RedirectToAction(nameof(Index));

        return Redirect(referer);
    }

    public IActionResult Fehler()
        => View();
}