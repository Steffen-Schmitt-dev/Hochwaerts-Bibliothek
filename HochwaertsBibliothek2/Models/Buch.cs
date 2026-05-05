using System.ComponentModel.DataAnnotations;

namespace HochwaertsBibliothek2.Models;

public class Buch
{
    public int BuchId { get; set; }

    [Required, StringLength(200)]
    public string Titel { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Autor { get; set; } = string.Empty;

    [Range(1450, 2100)]
    public int Erscheinungsjahr { get; set; }

    [Required, StringLength(20)]
    public string Isbn { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Klappentext { get; set; }

    public string? Inhalt { get; set; }

    public ICollection<Exemplar> Exemplare { get; set; } = new List<Exemplar>();
}