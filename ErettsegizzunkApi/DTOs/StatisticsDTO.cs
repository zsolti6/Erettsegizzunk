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

    public class PutStatisticsDTO
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public int SubjectId { get; set; }

        public Dictionary<int, bool> TaskIds { get; set; }
    }
}
