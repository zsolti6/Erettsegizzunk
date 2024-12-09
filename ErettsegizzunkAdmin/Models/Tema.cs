using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class Tema
{
    public int Id { get; set; }

    public string Nev { get; set; } = null!;

    public virtual ICollection<Feladatok> Feladatoks { get; set; } = new List<Feladatok>();
}
