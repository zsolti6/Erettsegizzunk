namespace ErettsegizzunkApi.Models;

public partial class SpacedRepetition
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TaskId { get; set; }

    public DateTime LastCorrectTime { get; set; }

    public int IntervalDays { get; set; }

    public DateTime NextDueTime { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
