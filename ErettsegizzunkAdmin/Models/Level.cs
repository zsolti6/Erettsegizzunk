using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class Level
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
