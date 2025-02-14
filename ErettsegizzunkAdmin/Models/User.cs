using ErettsegizzunkAdmin.Services;
using System.ComponentModel;

namespace ErettsegizzunkApi.Models;

public partial class User
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public string LoginName { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int PermissionId { get; set; }

    public bool Active { get; set; }

    public bool Newsletter { get; set; }

    public string? ProfilePicturePath { get; set; }

    public bool GoogleUser { get; set; }

    public DateTime? SignupDate { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual ICollection<SpacedRepetition> SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    public virtual ICollection<UserStatistic> UserStatistics { get; set; } = new List<UserStatistic>();

    public string? PermissionName { get { return Permission is null ? "" : JogosultsagList[PermissionId - 1]; } set { PermissionId = JogosultsagList.IndexOf(value) + 1; } }

    public List<string> JogosultsagList { get; private set; }// = new List<string>() { "Látogató", "Admin" };//databasből lekérni majd

    private void SetLists()
    {
        ApiService _apiService = new ApiService();

        JogosultsagList = _apiService.GetPermessionskAsync().Result.Select(x => x.Name).ToList();
    }
}
