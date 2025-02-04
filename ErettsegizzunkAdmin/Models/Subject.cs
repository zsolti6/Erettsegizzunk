﻿using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class Subject
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
