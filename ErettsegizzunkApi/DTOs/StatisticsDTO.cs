using ErettsegizzunkApi.Models;
using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkApi.DTOs
{
    public class GetAllStatisticsDTO
    {
        public string Token { get; set; }

        public int Mettol { get; set; }
    }

    public class FilteredTaksDTO
    {
        public Task Task { get; set; }

        public string UtolsoKitoltesDatum { get; set; }

        public bool UtolsoSikeres { get; set; }

        public int[] JoRossz { get; set; }
    }

    public class PostStatisticsDTO
    {
        public string Token { get; set; }

        public int UserId { get; set; }

        public Dictionary<int, bool> TaskIds { get; set; }
    }

    public class GetFillingCountDTO
    {
        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
