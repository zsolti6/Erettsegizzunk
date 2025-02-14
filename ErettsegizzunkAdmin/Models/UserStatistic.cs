namespace ErettsegizzunkApi.Models;

public partial class UserStatistic
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? StatisticsDate { get; set; }

    public string? MathSuccessfulTasks { get; set; }

    public string? MathUnsuccessfulTasks { get; set; }

    public string? HistorySuccessfulTasks { get; set; }

    public string? HistoryUnsuccessfulTasks { get; set; }

    public string? HungarianSuccessfulTasks { get; set; }

    public string? HungarianUnsuccessfulTasks { get; set; }

    public virtual User User { get; set; } = null!;
}
