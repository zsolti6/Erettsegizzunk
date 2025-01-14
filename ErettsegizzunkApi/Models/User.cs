using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string LoginName { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string? Name { get; set; }

    public int PermissionId { get; set; }

    public bool Active { get; set; }

    public string Email { get; set; } = null!;

    public string? ProfilePicturePath { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
