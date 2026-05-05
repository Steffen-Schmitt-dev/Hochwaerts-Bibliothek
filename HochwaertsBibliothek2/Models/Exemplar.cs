using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HochwaertsBibliothek2.Models;

public enum VerleihStatus
{
    Ausleihbar = 0,
    Verliehen = 1,
    Überprüfung = 2,
    Verschollen = 3,
    Verkaufbar = 4,
    Abschreibbar = 5
}

public enum Beschädigungsstufe
{
    Neu = 0,
    Super = 1,
    Gut = 2,
    Ausreichend = 3,
    Unzureichend = 4
}

public class Exemplar
{
    public int ExemplarId { get; set; }

    [ForeignKey(nameof(Buch))]
    public int BuchId { get; set; }

    public Buch? Buch { get; set; }

    public VerleihStatus Status { get; set; } = VerleihStatus.Ausleihbar;

    public Beschädigungsstufe Beschädigung { get; set; } = Beschädigungsstufe.Neu;

    public string? InventarCode { get; set; }

    public DateTime? VerschollenSeit { get; set; }

    public ICollection<Ausleihe> Ausleihen { get; set; } = new List<Ausleihe>();
}