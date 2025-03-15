namespace ErettsegizzunkApi.DTOs
{
    public class TaskGetRandomDTO
    {
        public string? Tantargy { get; set; } = null;

        public string? Szint { get; set; } = null;
    }

    public class TaskGetRandomFilterDTO //nem igazán használva átírni???
    {
        public string? Tantargy { get; set; } = null;

        public string? Szint { get; set; } = null;

        public int[] Themes { get; set; } 
    }

    public class TaskPutPostDTO : ParentDTO
    {
        public int? Id { get; set; }

        public string? KepNev { get; set; }

        public string? Leiras { get; set; }

        public string? Szoveg { get; set; }

        public string? Megoldasok { get; set; }

        public string? Helyese { get; set; }

        public int TantargyId { get; set; }

        public int TipusId { get; set; }

        public int SzintId { get; set; }

        public List<string> Temak { get; set; }
    }

}
