namespace ErettsegizzunkApi.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }

}
