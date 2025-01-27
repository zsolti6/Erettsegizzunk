using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string LoginName { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int PermissionId { get; set; }

    public bool Active { get; set; }

    public bool Newsletter { get; set; }

    public string? ProfilePicturePath { get; set; } = "default.jpg";

    public DateTime? SignupDate { get; set; }

    public virtual Permission? Permission { get; set; } = null!;

    public virtual ICollection<SpacedRepetition>? SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    public virtual ICollection<Token>? Tokens { get; set; } = new List<Token>();

    public virtual ICollection<UserStatistic>? UserStatistics { get; set; } = new List<UserStatistic>();
}
