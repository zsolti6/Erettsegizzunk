using ErettsegizzunkAdmin.Models;
using System.Windows.Media.Imaging;

namespace ErettsegizzunkAdmin.DTOs
{
    public class LoggedUserDTO : ParentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Permission { get; set; } = -1;

        public BitmapImage? ProfilePicture { get; set; } = null!;

        public string? ProfilePicturePath { get; set; } = null!;
    }

    public class LoggedUserForCheckDTO : ParentDTO
    {
        public int Id { get; set; }

        public int Permission { get; set; }
    }

    public class FelhasznaloModotsitDTO : ParentDTO
    {
        public List<User> users { get; set; }
    }
}
