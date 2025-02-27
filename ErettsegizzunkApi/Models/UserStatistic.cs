using System;
using System.Collections.Generic;

namespace ErettsegizzunkApi.Models;

public partial class UserStatistic
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? StatisticsDate { get; set; }

    public string MathSuccessfulTasks { get; set; } = string.Empty;

    public string MathUnsuccessfulTasks { get; set; } = string.Empty;

    public string HistorySuccessfulTasks { get; set; } = string.Empty;

    public string HistoryUnsuccessfulTasks { get; set; } = string.Empty;

    public string HungarianSuccessfulTasks { get; set; } = string.Empty;

    public string HungarianUnsuccessfulTasks { get; set; } = string.Empty;

    public virtual User User { get; set; } = null!;
}
