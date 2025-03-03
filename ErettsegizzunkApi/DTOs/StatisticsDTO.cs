using ErettsegizzunkApi.Models;
using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkApi.DTOs
{
    public class GetAllStatisticsDTO
    {
        public string Token { get; set; }

        public int Mettol { get; set; }
    }

    public class GetOneStatisticsDTO
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public int[] SubjectIds { get; set; }

        public int[] ThemeIds { get; set; }

    }

    public class GetOneFilterStatisticsDTO
    {
        public int Id { get; set; }

        public int OldalSzam { get; set; }

        public int[] SzintId { get; set; }

        public int SubjectId { get; set; }

        public string Token { get; set; }

    }

    public class FilteredTaksDTO
    {
        public Task Task { get; set; }

        public int[] JoRossz { get; set; }
    }

    public class PutStatisticsDTO
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public int SubjectId { get; set; }

        public Dictionary<int, bool> TaskIds { get; set; }
    }

    public class GetFillingByDateDTO //Dátumut berakni ide ha szűrni kell
    {
        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
