using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class Token
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Token1 { get; set; }

    public bool? Aktiv { get; set; }

    public DateTime? Login { get; set; }

    public DateTime? Logout { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }//??ignor??
}
