using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class LoggedUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Permission { get; set; }
        public bool Newsletter { get; set; }
        public byte[]? ProfilePicture { get; set; } = null;
        public string? ProfilePicturePath { get; set; } = null!;
        public string Token { get; set; }
    }

    public class LoggedUserForCheckDTO
    {
        public int Id { get; set; }
        public int Permission { get; set; }
        public string Token { get; set; }
    }

    public class FelhasznaloTorolDTO
    {
        public string Token { get; set; }
        public List<int> Ids { get; set; }
    }

    public class FelhasznaloModotsitDTO
    {
        public List<User> Users { get; set; } = null!;
        public string? Token { get; set; }
    }
}
