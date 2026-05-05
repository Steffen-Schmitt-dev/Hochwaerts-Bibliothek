using HochwaertsBibliothek2.Data;
using HochwaertsBibliothek2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HochwaertsBibliothek2.Controllers;

public class BuecherController : Controller
{
    private readonly BibliothekDbContext _db;

    public BuecherController(BibliothekDbContext db)
        => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.Buecher.OrderBy(b => b.Titel).ToListAsync());

    public IActionResult Ausleihe()
        => View();

    public IActionResult Pruefung()
        => View();

    public async Task<IActionResult> Details(int id)
    {
        var buch = await _db.Buecher
            .Include(b => b.Exemplare)
            .FirstOrDefaultAsync(b => b.BuchId == id);

        if (buch == null) return NotFound();

        return View(buch);
    }

    public IActionResult Erstellen()
        => View(new Buch());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Erstellen(Buch buch)
    {
        if (!ModelState.IsValid)
            return View(buch);

        _db.Buecher.Add(buch);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = buch.BuchId });
    }

    public async Task<IActionResult> Bearbeiten(int id)
    {
        var buch = await _db.Buecher.FindAsync(id);

        if (buch == null) return NotFound();

        return View(buch);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Bearbeiten(int id, Buch buch)
    {
        if (id != buch.BuchId) return BadRequest();
        if (!ModelState.IsValid) return View(buch);

        var existing = await _db.Buecher.FindAsync(id);
        if (existing == null) return NotFound();

        _db.Entry(existing).CurrentValues.SetValues(buch);

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = buch.BuchId });
    }

    public async Task<IActionResult> Loeschen(int id)
    {
        var buch = await _db.Buecher.FindAsync(id);

        if (buch == null) return NotFound();

        return View(buch);
    }

    [HttpPost, ActionName("Loeschen")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoeschenBestaetigt(int id)
    {
        var buch = await _db.Buecher.FindAsync(id);

        if (buch == null) return NotFound();

        _db.Buecher.Remove(buch);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}