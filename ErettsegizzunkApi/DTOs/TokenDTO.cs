namespace ErettsegizzunkApi.DTOs
{
    public class AddTokenDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
    }

    public class ModifyToken
    {
        public int Id { get; set; }
        public DateTime LogOut { get; set; }
        public bool Aktiv { get; set; }
    }
}
