using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class FeladatokPutPostDTO
    {
        public int? Id { get; set; }
        public string Token { get; set; } = "";

        public string? KepNev { get; set; }

        public string? Leiras { get; set; }
        
        public string? Szoveg { get; set; }

        public string? Megoldasok { get; set; }

        public string? Helyese { get; set; }

        public int TantargyId { get; set; }

        public int TipusId { get; set; }

        public int SzintId { get; set; }

        public string[] Temak { get; set; }
    }

    public class FeladatokDeleteDTO
    {
        public string Token { get; set; } = "";

        public List<int> Ids { get; set; }
    }

}
