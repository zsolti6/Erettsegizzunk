using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class Permission
{
    public int Id { get; set; }

    public int? Level { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
