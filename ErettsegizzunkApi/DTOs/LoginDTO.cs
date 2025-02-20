using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class LoginDTO
    {
        public string LoginName { get; set; }
        public string TmpHash { get; set; }
    }

    public class LoginRegistryRequestDTO
    {
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? CaptchaToken { get; set; }
        public User? User { get; set; } = null;
    }
}
