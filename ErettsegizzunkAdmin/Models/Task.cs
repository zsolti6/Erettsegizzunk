namespace ErettsegizzunkAdmin.Models;

public partial class Task
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Text { get; set; }

    public string? Answers { get; set; }

    public string? IsCorrect { get; set; }

    public int SubjectId { get; set; }

    public int TypeId { get; set; }

    public int LevelId { get; set; }

    public string? PicName { get; set; } = null;

    public virtual Level? Level { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Type? Type { get; set; }

    public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();

    public virtual ICollection<SpacedRepetition> SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    public string SubjectName { get { return Subject.Name; } set { SubjectId = SubjectList.IndexOf(value) + 1; } }

    public string LevelName { get { return Level.Name; } set { LevelId = LevelList.IndexOf(value) + 1; } }

    public string TypeName { get { return Type.Name; } set { TypeId = TypeList.IndexOf(value) + 1; } }

    public List<string> SubjectList { get; set; }

    public List<string> LevelList { get; set; }

    public List<string> TypeList { get; set; }

    public List<string> ThemeList { get; set; }
}
