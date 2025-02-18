namespace ErettsegizzunkApi.DTOs
{
    public class GetAllStatisticsDTO
    {
        //public 
    }

    public class GetOneStatisticsDTO
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public int[] SubjectIds { get; set; }

        public int[] ThemeIds { get; set; }

    }
}
