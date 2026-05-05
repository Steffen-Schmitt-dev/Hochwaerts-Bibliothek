using HochwaertsBibliothek2.Data;
using HochwaertsBibliothek2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HochwaertsBibliothek2.Controllers;

public class ExemplareController : Controller
{
    private readonly BibliothekDbContext _db;

    public ExemplareController(BibliothekDbContext db)
        => _db = db;

    public async Task<IActionResult> Index()
    {
        var exemplare = await _db.Exemplare
            .Include(e => e.Buch)
            .OrderBy(e => e.Buch!.Titel)
            .ThenBy(e => e.ExemplarId)
            .ToListAsync();

        return View(exemplare);
    }

    public async Task<IActionResult> Details(int id)
    {
        var exemplar = await _db.Exemplare
            .Include(e => e.Buch)
            .Include(e => e.Ausleihen.OrderByDescending(a => a.AusleihDatum))
            .FirstOrDefaultAsync(e => e.ExemplarId == id);

        if (exemplar == null) return NotFound();

        return View(exemplar);
    }

    public async Task<IActionResult> Registrieren(int buchId)
    {
        var buch = await _db.Buecher.FindAsync(buchId);
        if (buch == null) return NotFound();

        var exemplar = new Exemplar
        {
            BuchId = buchId,
            Status = VerleihStatus.Ausleihbar,
            Beschädigung = Beschädigungsstufe.Neu
        };

        _db.Exemplare.Add(exemplar);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = exemplar.ExemplarId });
    }

    public async Task<IActionResult> Verleihen(int id)
    {
        var exemplar = await _db.Exemplare
            .Include(e => e.Buch)
            .FirstOrDefaultAsync(e => e.ExemplarId == id);

        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Ausleihbar)
        {
            TempData["Fehler"] = "Dieses Exemplar ist aktuell nicht ausleihbar.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(exemplar);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Verleihen(int exemplarId, string ausleiherName)
    {
        if (string.IsNullOrWhiteSpace(ausleiherName))
        {
            ModelState.AddModelError("ausleiherName", "Bitte einen Namen eingeben.");
        }

        var exemplar = await _db.Exemplare
            .Include(e => e.Buch)
            .FirstOrDefaultAsync(e => e.ExemplarId == exemplarId);

        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Ausleihbar)
        {
            TempData["Fehler"] = "Exemplar ist nicht ausleihbar.";
            return RedirectToAction(nameof(Details), new { id = exemplarId });
        }

        if (!ModelState.IsValid)
            return View(exemplar);

        var jetzt = DateTime.UtcNow;

        var ausleihe = new Ausleihe
        {
            ExemplarId = exemplarId,
            AusleiherName = ausleiherName.Trim(),
            AusleihDatum = jetzt,
            Mahnstufe = 0,
            LetzteMahnungAm = null,
            RueckgabeDatum = null
        };

        exemplar.Status = VerleihStatus.Verliehen;

        _db.Ausleihen.Add(ausleihe);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = exemplarId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rueckgabe(int id)
    {
        var exemplar = await _db.Exemplare
            .Include(e => e.Ausleihen)
            .FirstOrDefaultAsync(e => e.ExemplarId == id);

        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Verliehen &&
            exemplar.Status != VerleihStatus.Verschollen)
        {
            TempData["Fehler"] =
                "Rückgabe nur bei verliehen oder verschollen möglich.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var aktiveAusleihe = exemplar.Ausleihen
            .OrderByDescending(a => a.AusleihDatum)
            .FirstOrDefault(a => a.RueckgabeDatum == null);

        if (aktiveAusleihe == null)
        {
            TempData["Fehler"] = "Keine aktive Ausleihe gefunden.";
            return RedirectToAction(nameof(Details), new { id });
        }

        aktiveAusleihe.RueckgabeDatum = DateTime.UtcNow;

        exemplar.VerschollenSeit = null;
        exemplar.Status = VerleihStatus.Überprüfung;

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Pruefen), new { id });
    }

    public async Task<IActionResult> Pruefen(int id)
    {
        var exemplar = await _db.Exemplare
            .Include(e => e.Buch)
            .FirstOrDefaultAsync(e => e.ExemplarId == id);

        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Überprüfung)
        {
            TempData["Fehler"] = "Nur im Status 'Überprüfung' möglich.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(exemplar);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pruefen(int exemplarId, Beschädigungsstufe neueBeschaedigung)
    {
        var exemplar = await _db.Exemplare.FindAsync(exemplarId);
        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Überprüfung)
        {
            TempData["Fehler"] = "Nur im Status 'Überprüfung' möglich.";
            return RedirectToAction(nameof(Details), new { id = exemplarId });
        }

        exemplar.Beschädigung = neueBeschaedigung;

        exemplar.Status = exemplar.Beschädigung == Beschädigungsstufe.Unzureichend
            ? VerleihStatus.Verkaufbar
            : VerleihStatus.Ausleihbar;

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = exemplarId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Verkauft(int id)
    {
        var exemplar = await _db.Exemplare.FindAsync(id);
        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Verkaufbar)
        {
            TempData["Fehler"] = "Nur 'Verkaufbar' möglich.";
            return RedirectToAction(nameof(Details), new { id });
        }

        _db.Exemplare.Remove(exemplar);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Abschreiben(int id)
    {
        var exemplar = await _db.Exemplare.FindAsync(id);
        if (exemplar == null) return NotFound();

        if (exemplar.Status != VerleihStatus.Abschreibbar)
        {
            TempData["Fehler"] = "Nicht abschreibbar.";
            return RedirectToAction(nameof(Details), new { id });
        }

        _db.Exemplare.Remove(exemplar);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Ausleihe()
    {
        var exemplare = await _db.Exemplare
            .Include(e => e.Buch)
            .Where(e => e.Status == VerleihStatus.Ausleihbar)
            .OrderBy(e => e.Buch!.Titel)
            .ToListAsync();

        return View(exemplare);
    }

    public async Task<IActionResult> Ueberpruefung()
    {
        var exemplare = await _db.Exemplare
            .Include(e => e.Buch)
            .Where(e => e.Status == VerleihStatus.Überprüfung)
            .OrderBy(e => e.Buch!.Titel)
            .ToListAsync();

        return View(exemplare);
    }
}