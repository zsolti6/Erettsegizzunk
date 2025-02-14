using ErettsegizzunkAdmin.Services;

namespace ErettsegizzunkApi.Models;

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

    public string? PicName { get; set; }

    public virtual Level? Level { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Type? Type { get; set; }

    public virtual ICollection<MaterialDesignThemes.Wpf.Theme> Themes { get; set; } = new List<MaterialDesignThemes.Wpf.Theme>();

    public virtual ICollection<SpacedRepetition> SpacedRepetitions { get; set; } = new List<SpacedRepetition>();

    public string SubjectName { get { return Subject.Name; } set { SubjectId = SubjectList.IndexOf(value) + 1; } }

    public string LevelName { get { return Level.Name; } set { LevelId = LevelList.IndexOf(value) + 1; } }

    public string TypeName { get { return Type.Name; } set { TypeId = TypeList.IndexOf(value) + 1; } }

    //databaseből lekérni majd
    public List<string> SubjectList { get; private set; }//new List<string>() { "matematika", "történelem", "magyar" };

    public List<string> LevelList { get; private set; } //= new List<string>() { "közép", "emelt" };

    public List<string> TypeList { get; private set; } //= new List<string>() { "radio", "chechkbox", "textbox" };
    //MOST

    public Task()
    {
        SetLists();
    }

    private void SetLists()
    {
        ApiService _apiService = new ApiService();

        SubjectList = _apiService.GetTantargyaksAsync().Result.Select(x => x.Name).ToList();
        LevelList = _apiService.GetLevelAsync().Result.Select(x => x.Name).ToList();
        TypeList = _apiService.GetTipusAsync().Result.Select(x => x.Name).ToList();
    }
}
