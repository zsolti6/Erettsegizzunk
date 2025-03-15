using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkApi.DTOs
{
    public class StatisticsResetDTO : ParentDTO //delete
    {
        public int UserId { get; set; }
    }

    public class DeatiledStatisticsDTO : ParentDTO
    {
        public int UserId { get; set; }

        public int Oldal { get; set; }
    }

    public class FilteredDeatiledStatisticsDTO : ParentDTO
    {
        public int UserId { get; set; }

        public int Oldal { get; set; }

        public string Szoveg { get; set; }

        public int ThemeId { get; set; }

        public int LevelId { get; set; }

        public int SubjectId { get; set; }
    }


    public class FilteredTaskDTO
    {
        public Task Task { get; set; }

        public DateTime UtolsoKitoltesDatum { get; set; }

        public bool UtolsoSikeres { get; set; }

        public int[] JoRossz { get; set; }
    }

    public class FilteredTaskLessDTO
    {
        public Task Task { get; set; }

        public DateTime UtolsoKitoltesDatum { get; set; }

        public bool UtolsoSikeres { get; set; }
    }

    public class PostStatisticsDTO : ParentDTO
    {
        public int UserId { get; set; }

        public Dictionary<int, bool> TaskIds { get; set; }
    }

    public class GetFillingCountDTO : ParentDTO
    {
        public int UserId { get; set; }
    }
}
