using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.Models;

public partial class Task
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Text { get; set; }

    public string? Answers { get; set; }

    public string? IsCorrect { get; set; }

    public int? SubjectId { get; set; }

    public int? TypeId { get; set; }

    public int? LevelId { get; set; }

    public string? PicName { get; set; }

    public virtual Level? Level { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Type? Type { get; set; }

    public virtual ICollection<MaterialDesignThemes.Wpf.Theme> Themes { get; set; } = new List<MaterialDesignThemes.Wpf.Theme>();

    public virtual ICollection<SpacedRepetition> SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    public string SubjectName { get { return Subject.Name; } private set { } }
    public string LevelName { get { return Level.Name; } private set { } }
    public string TypeName { get { return Type.Name; } private set {  } }
}
