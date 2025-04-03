using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkApi.DTOs
{
    public class StatisticsResetDTO : ParentUserCheckDTO //delete statistics
    {
    }

    public class DeatiledStatisticsDTO : ParentUserCheckDTO
    {
        public int Oldal { get; set; }
    }

    public class FilteredDeatiledStatisticsDTO : ParentUserCheckDTO
    {
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

    public class FilteredTaskCountDTO
    {
        public List<FilteredTaskDTO> FilteredTasks { get; set; }

        public double OldalDarab { get; set; }
    }

    public class FilteredTaskLessDTO
    {
        public Task Task { get; set; }

        public DateTime UtolsoKitoltesDatum { get; set; }

        public bool UtolsoSikeres { get; set; }
    }

    public class PostStatisticsDTO : ParentUserCheckDTO
    {
        public Dictionary<int, bool> TaskIds { get; set; }
    }

    public class GetFillingCountDTO : ParentUserCheckDTO
    {
    }
}
