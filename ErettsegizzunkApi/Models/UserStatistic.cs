using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class UserStatistic
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TaskId { get; set; }

    public bool IsSuccessful { get; set; }

    public DateTime FilloutDate { get; set; }

    public virtual Task Task { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
