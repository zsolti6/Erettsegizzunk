﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class Tipus
{
    public int Id { get; set; }

    public string Nev { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Feladatok> Feladatoks { get; set; } = new List<Feladatok>();
}