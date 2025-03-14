using ErettsegizzunkApi.Models;
using System.Text.Json.Serialization;

namespace ErettsegizzunkApi.DTOs
{
    public class SzurtTemaDTO
    {
        [JsonIgnore]
        public string SubjectName { get; set; }

        public Theme Theme { get; set; }

        public int Count { get; set; }
    }
}
