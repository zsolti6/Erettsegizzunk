using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class Feladatok
{
    public bool Kijelolve { get; set; }
    public int Id { get; set; }

    public string? Leiras { get; set; }

    public string? Megoldasok { get; set; }

    public string? Helyese { get; set; }

    public int? TantargyId { get; set; }

    public int? TipusId { get; set; }

    public int? SzintId { get; set; }

    public virtual Szint? Szint { get; set; }

    public virtual Tantargyak? Tantargy { get; set; }

    public virtual Tipu? Tipus { get; set; }

    public virtual ICollection<Tema> Temas { get; set; } = new List<Tema>();
}
