using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class Task
{
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

    public virtual ICollection<SpacedRepetition> SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    public virtual Subject? Subject { get; set; }

    public virtual Type? Type { get; set; }

    public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();
}
