namespace ErettsegizzunkApi.Models;

public partial class Theme
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
