namespace ErettsegizzunkAdmin.DTOs
{
    public class ParentDTO
    {
        public string Token { get; set; } = "";
    }

    public class ParentDeleteDTO : ParentDTO
    {
        public List<int> Ids { get; set; } = null!;
    }
}
