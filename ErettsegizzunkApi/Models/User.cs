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

    public string Email { get; set; } = null!;

    public int PermissionId { get; set; }

    public bool Active { get; set; }

    public bool Newsletter { get; set; }

    public string? ProfilePicturePath { get; set; }

    public DateTime SignupDate { get; set; }

    public bool GoogleUser { get; set; }

    public virtual Permission? Permission { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<SpacedRepetition> SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    [JsonIgnore] //--> kell?
    public virtual ICollection<UserStatistic> UserStatistics { get; set; } = new List<UserStatistic>();
}
