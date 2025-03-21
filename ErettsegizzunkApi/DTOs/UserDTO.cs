using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class LoggedUserDTO : ParentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Permission { get; set; }

        public bool Newsletter { get; set; }

        public byte[]? ProfilePicture { get; set; } = null;

        public string? ProfilePicturePath { get; set; } = null!;

        public bool GoogleUser { get; set; }
    }

    public class LoggedUserForCheckDTO : ParentDTO
    {
        public int userId { get; set; }

        public int Permission { get; set; }
    }

    public class UserPutTO : ParentDTO
    {
        public List<User> Users { get; set; } = null!;
    }
}
