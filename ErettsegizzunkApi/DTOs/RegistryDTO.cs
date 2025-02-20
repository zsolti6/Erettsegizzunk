using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class RegistryDTO
    {
        public int Id { get; set; }

        public string LoginName { get; set; } = null!;

        public string Hash { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public string? Name { get; set; }

        public int PermissionId { get; set; }

        public bool Active { get; set; }

        public string Email { get; set; } = null!;

        public string? ProfilePicturePath { get; set; }
    }

    public class CaptchaRegisrtyDTO
    {
        public User user { get; set; }

        public string CaptchaToken { get; set; }
    }
}
