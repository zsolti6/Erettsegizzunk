﻿using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class Permission
{
    public int Id { get; set; }

    public int Level { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}