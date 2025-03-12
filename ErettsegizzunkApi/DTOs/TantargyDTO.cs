using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class TantargyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Token { get; set; } = null!;
    }

    public class TantargyDeleteDTO
    {
        public string Token { get; set; } = null!;

        public List<int> Ids { get; set; }
    }

    public class TantargyPutDTO
    {
        public string Token { get; set; } = null!;

        public List<Subject> subjects { get; set; }
    }
}
