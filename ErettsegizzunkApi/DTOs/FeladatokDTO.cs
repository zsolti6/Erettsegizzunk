namespace ErettsegizzunkApi.DTOs
{
    public class FeladatokGetRandomDTO
    {
        public string? Tantargy { get; set; } = null;

        public string? Szint { get; set; } = null;
    }

    public class FeladatokGetRandomSzuresDTO //nem igazán használva átírni???
    {
        public string? Tantargy { get; set; } = null;

        public string? Szint { get; set; } = null;

        public int[] Themes { get; set; } 
    }

    public class FeladatokPutPostDTO
    {
        public int? Id { get; set; }

        public string Token { get; set; }

        public string? KepNev { get; set; }

        public string? Leiras { get; set; }

        public string? Szoveg { get; set; }

        public string? Megoldasok { get; set; }

        public string? Helyese { get; set; }

        public int TantargyId { get; set; }

        public int TipusId { get; set; }

        public int SzintId { get; set; }
    }

    public class FeladatokDeleteDTO
    {
        public string Token { get; set; }

        public List<int> Ids { get; set; }
    }

}
