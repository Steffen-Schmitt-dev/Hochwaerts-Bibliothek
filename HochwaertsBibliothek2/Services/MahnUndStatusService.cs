using HochwaertsBibliothek2.Data;
using HochwaertsBibliothek2.Models;
using Microsoft.EntityFrameworkCore;

namespace HochwaertsBibliothek2.Services;

public class MahnUndStatusService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MahnUndStatusService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Aktualisieren(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            await Aktualisieren(stoppingToken);
        }
    }

    private async Task Aktualisieren(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BibliothekDbContext>();

        var jetzt = DateTime.UtcNow;

        var aktiveAusleihen = await db.Ausleihen
            .Include(a => a.Exemplar)
            .Where(a => a.RueckgabeDatum == null)
            .ToListAsync(ct);

        foreach (var ausleihe in aktiveAusleihen)
        {
            if (ausleihe.Exemplar == null) continue;
            if (ausleihe.Exemplar.Status != VerleihStatus.Verliehen) continue;

            var tageSeitAusleihe = (jetzt.Date - ausleihe.AusleihDatum.Date).Days;

            if (tageSeitAusleihe < 30)
                continue;

            var berechneteMahnstufe = tageSeitAusleihe switch
            {
                >= 90 => Mahnstufe.Stufe3,
                >= 60 => Mahnstufe.Stufe2,
                _ => Mahnstufe.Stufe1
            };

            if (berechneteMahnstufe > ausleihe.Mahnstufe)
            {
                ausleihe.Mahnstufe = berechneteMahnstufe;
                ausleihe.LetzteMahnungAm = jetzt;
            }

            if (ausleihe.Mahnstufe >= Mahnstufe.Stufe3)
            {
                if (ausleihe.Exemplar.Status != VerleihStatus.Verschollen)
                {
                    ausleihe.Exemplar.Status = VerleihStatus.Verschollen;
                    ausleihe.Exemplar.VerschollenSeit ??= jetzt;
                }
            }
        }

        var verschollene = await db.Exemplare
            .Where(e => e.Status == VerleihStatus.Verschollen
                     && e.VerschollenSeit != null)
            .ToListAsync(ct);

        foreach (var exemplar in verschollene)
        {
            if (exemplar.VerschollenSeit!.Value <= jetzt.AddYears(-1))
            {
                exemplar.Status = VerleihStatus.Abschreibbar;
            }
        }

        await db.SaveChangesAsync(ct);
    }
}