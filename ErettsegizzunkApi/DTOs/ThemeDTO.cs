using ErettsegizzunkApi.Models;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.DTOs
{
    public class ThemeFilteredDTO
    {
        [JsonIgnore]
        public string SubjectName { get; set; }

        public Theme Theme { get; set; }

        public int Count { get; set; }
    }

    public class PutPostThemeDTO : ParentDTO
    {
        public Theme Theme { get; set; }
    }
}
