using HochwaertsBibliothek2.Models;
using Microsoft.EntityFrameworkCore;

namespace HochwaertsBibliothek2.Data;

public class BibliothekDbContext : DbContext
{
    public BibliothekDbContext(DbContextOptions<BibliothekDbContext> options)
        : base(options)
    {
    }

    public DbSet<Buch> Buecher => Set<Buch>();
    public DbSet<Exemplar> Exemplare => Set<Exemplar>();
    public DbSet<Ausleihe> Ausleihen => Set<Ausleihe>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Buch>()
            .HasIndex(b => b.Isbn);

        modelBuilder.Entity<Exemplar>()
            .HasOne(e => e.Buch)
            .WithMany(b => b.Exemplare)
            .HasForeignKey(e => e.BuchId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ausleihe>()
            .HasOne(a => a.Exemplar)
            .WithMany(e => e.Ausleihen)
            .HasForeignKey(a => a.ExemplarId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Exemplar>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Exemplar>()
            .Property(e => e.Beschädigung)
            .HasConversion<int>();

        var seedDatum = new DateTime(2026, 03, 01, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Buch>().HasData(
            new Buch
            {
                BuchId = 1,
                Titel = "Zauberhafte Datenbanken",
                Autor = "Irma Pince",
                Erscheinungsjahr = 2021,
                Isbn = "978-0000000001",
                Klappentext = "Ein Leitfaden für relationale Magie in Tabellenform."
            },
            new Buch
            {
                BuchId = 2,
                Titel = "MVC für Muggel",
                Autor = "Prof. Wandermann",
                Erscheinungsjahr = 2023,
                Isbn = "978-0000000002",
                Klappentext = "Model, View, Controller – ganz ohne Zauberstab."
            },
            new Buch
            {
                BuchId = 3,
                Titel = "Buchstabeneulen & Bibliothekslogik",
                Autor = "Eleonore Federkiel",
                Erscheinungsjahr = 2020,
                Isbn = "978-0000000003",
                Klappentext = "Zustände elegant verwalten: Ausleihbar bis Verkaufbar."
            },
            new Buch
            {
                BuchId = 4,
                Titel = "Hexenwerk mit C#",
                Autor = "Klaus Spellman",
                Erscheinungsjahr = 2024,
                Isbn = "978-0000000004",
                Klappentext = "C#-Zaubertricks für sauberen Code."
            },
            new Buch
            {
                BuchId = 5,
                Titel = "Die Kunst der Mahnung",
                Autor = "Ilias Ink",
                Erscheinungsjahr = 2022,
                Isbn = "978-0000000005",
                Klappentext = "Fristen, Erinnerungen und Ordnung in der Bibliothek."
            },
            new Buch
            {
                BuchId = 6,
                Titel = "Bootstrap & Bannergold",
                Autor = "Stefan Müller",
                Erscheinungsjahr = 2025,
                Isbn = "978-0000000006",
                Klappentext = "UI, das nach Hochwärts aussieht – ohne CSS-Schmerz."
            },
            new Buch
            {
                BuchId = 7,
                Titel = "EF Core Codex",
                Autor = "Valeria Wurm",
                Erscheinungsjahr = 2021,
                Isbn = "978-0000000007",
                Klappentext = "Code-First, Migrationen und magische Daten."
            },
            new Buch
            {
                BuchId = 8,
                Titel = "Die Leseratte liest",
                Autor = "Robin Rot",
                Erscheinungsjahr = 2019,
                Isbn = "978-0000000008",
                Klappentext = "Kurzgeschichten aus dem Roten Haus."
            },
            new Buch
            {
                BuchId = 9,
                Titel = "Vorlesebär Bruno",
                Autor = "Bruno Grün",
                Erscheinungsjahr = 2018,
                Isbn = "978-0000000009",
                Klappentext = "Geschichten für lange Winterabende."
            },
            new Buch
            {
                BuchId = 10,
                Titel = "Zauberschule der Tabellen",
                Autor = "Archiv Hochwärts",
                Erscheinungsjahr = 2017,
                Isbn = "978-0000000010",
                Klappentext = "Von ERM bis PK/FK – Grundlagen der Ordnung."
            },
            new Buch
            {
                BuchId = 11,
                Titel = "Das große Klappentext-Kompendium",
                Autor = "Bibliotheksrat",
                Erscheinungsjahr = 2016,
                Isbn = "978-0000000011",
                Klappentext = "Wenn der Text hinten besser ist als vorne."
            },
            new Buch
            {
                BuchId = 12,
                Titel = "Eule Eleonore erklärt UML",
                Autor = "Eleonore Federkiel",
                Erscheinungsjahr = 2022,
                Isbn = "978-0000000012",
                Klappentext = "Use Cases, Klassen, Zustände – verständlich."
            }
        );

        modelBuilder.Entity<Exemplar>().HasData(
            new Exemplar { ExemplarId = 101, BuchId = 1, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-DB-101" },
            new Exemplar { ExemplarId = 102, BuchId = 1, Status = VerleihStatus.Verkaufbar, Beschädigung = Beschädigungsstufe.Unzureichend, InventarCode = "HW-DB-102" },
            new Exemplar { ExemplarId = 103, BuchId = 1, Status = VerleihStatus.Verliehen, Beschädigung = Beschädigungsstufe.Super, InventarCode = "HW-DB-103" },
            new Exemplar { ExemplarId = 201, BuchId = 2, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-MVC-201" },
            new Exemplar { ExemplarId = 202, BuchId = 2, Status = VerleihStatus.Überprüfung, Beschädigung = Beschädigungsstufe.Gut, InventarCode = "HW-MVC-202" },
            new Exemplar { ExemplarId = 301, BuchId = 3, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Super, InventarCode = "HW-LOG-301" },
            new Exemplar { ExemplarId = 302, BuchId = 3, Status = VerleihStatus.Verkaufbar, Beschädigung = Beschädigungsstufe.Unzureichend, InventarCode = "HW-LOG-302" },
            new Exemplar { ExemplarId = 401, BuchId = 4, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-CS-401" },
            new Exemplar { ExemplarId = 402, BuchId = 4, Status = VerleihStatus.Verliehen, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-CS-402" },
            new Exemplar { ExemplarId = 501, BuchId = 5, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-MAH-501" },
            new Exemplar { ExemplarId = 502, BuchId = 5, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Ausreichend, InventarCode = "HW-MAH-502" },
            new Exemplar { ExemplarId = 601, BuchId = 6, Status = VerleihStatus.Überprüfung, Beschädigung = Beschädigungsstufe.Super, InventarCode = "HW-BS-601" },
            new Exemplar { ExemplarId = 602, BuchId = 6, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-BS-602" },
            new Exemplar { ExemplarId = 701, BuchId = 7, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-EF-701" },
            new Exemplar
            {
                ExemplarId = 702,
                BuchId = 7,
                Status = VerleihStatus.Verschollen,
                Beschädigung = Beschädigungsstufe.Gut,
                InventarCode = "HW-EF-702",
                VerschollenSeit = new DateTime(2024, 02, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Exemplar { ExemplarId = 801, BuchId = 8, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-ROT-801" },
            new Exemplar { ExemplarId = 901, BuchId = 9, Status = VerleihStatus.Verkaufbar, Beschädigung = Beschädigungsstufe.Unzureichend, InventarCode = "HW-GRN-901" },
            new Exemplar { ExemplarId = 1001, BuchId = 10, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-ERM-1001" },
            new Exemplar { ExemplarId = 1101, BuchId = 11, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-KT-1101" },
            new Exemplar { ExemplarId = 1201, BuchId = 12, Status = VerleihStatus.Verliehen, Beschädigung = Beschädigungsstufe.Super, InventarCode = "HW-UML-1201" },
            new Exemplar { ExemplarId = 1202, BuchId = 12, Status = VerleihStatus.Ausleihbar, Beschädigung = Beschädigungsstufe.Neu, InventarCode = "HW-UML-1202" }
        );

        modelBuilder.Entity<Ausleihe>().HasData(
            new Ausleihe
            {
                AusleiheId = 9001,
                ExemplarId = 103,
                AusleiherName = "Max Mustermann",
                AusleihDatum = seedDatum.AddDays(-10),
                RueckgabeDatum = null,
                Mahnstufe = 0,
                LetzteMahnungAm = null
            },
            new Ausleihe
            {
                AusleiheId = 9002,
                ExemplarId = 402,
                AusleiherName = "Erika Beispiel",
                AusleihDatum = seedDatum.AddDays(-35),
                RueckgabeDatum = null,
                Mahnstufe = 0,
                LetzteMahnungAm = null
            },
            new Ausleihe
            {
                AusleiheId = 9003,
                ExemplarId = 1201,
                AusleiherName = "Tim Test",
                AusleihDatum = seedDatum.AddDays(-95),
                RueckgabeDatum = null,
                Mahnstufe = 0,
                LetzteMahnungAm = null
            }
        );
    }
}