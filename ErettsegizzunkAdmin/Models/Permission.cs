using System.Text.Json.Serialization;

namespace ErettsegizzunkAdmin.Models;

public partial class Permission
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public int Level { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
