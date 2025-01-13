namespace ErettsegizzunkApi.DTOs
{
    public class LoggedUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Permission { get; set; } = null;//vagy semmi
        public string? ProfilePicturePath { get; set; } = null;
        public string Token { get; set; }
    }
}
