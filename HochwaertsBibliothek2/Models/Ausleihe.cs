namespace HochwaertsBibliothek2.Models;

public enum Mahnstufe
{
    Keine = 0,
    Stufe1 = 1,
    Stufe2 = 2,
    Stufe3 = 3
}

public class Ausleihe
{
    public int AusleiheId { get; set; }

    public int ExemplarId { get; set; }
    public Exemplar? Exemplar { get; set; }

    public string AusleiherName { get; set; } = string.Empty;

    public DateTime AusleihDatum { get; set; }

    public DateTime? RueckgabeDatum { get; set; }

    public Mahnstufe Mahnstufe { get; set; } = Mahnstufe.Keine;

    public DateTime? LetzteMahnungAm { get; set; }
}