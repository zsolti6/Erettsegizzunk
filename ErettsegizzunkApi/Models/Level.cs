using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class Level
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
