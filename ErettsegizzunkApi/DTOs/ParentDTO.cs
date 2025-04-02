namespace ErettsegizzunkApi.DTOs
{
    public class ParentDTO
    {
        public string Token { get; set; } = "";
    }

    public class ParentDeleteDTO : ParentDTO
    {
        public List<int> Ids { get; set; } = null!;
    }

    public class ParentUserCheckDTO : ParentDTO
    {
        public int UserId { get; set; }
    }
}
