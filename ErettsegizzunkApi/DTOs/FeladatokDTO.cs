namespace ErettsegizzunkApi.DTOs
{
    public class FeladatokGetRandomDTO
    {
        public string? Tantargy { get; set; } = null;

        public string? Szint { get; set; } = null;
    }

    public class FeladatokGetSpecificDTO //nem igazán használva átírni???
    {
        public int? Id { get; set; } = null;

        public string? Tantargy { get; set; } = null;

        public string? Szint { get; set; } = null;
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
