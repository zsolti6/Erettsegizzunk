using System.Windows.Media.Imaging;

namespace ErettsegizzunkApi.DTOs
{
    public class LoggedUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Permission { get; set; } = null;//vagy semmi
        public BitmapImage? ProfilePicture { get; set; } = null;
        public string? ProfilePicturePath { get; set; } = null;
        public string Token { get; set; }
    }
}
