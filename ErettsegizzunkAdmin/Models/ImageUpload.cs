using Microsoft.AspNetCore.Http;

namespace ErettsegizzunkAdmin.DTOs
{
    public class ImageUpload
    {
        public string Token { get; set; }

        public IFormFile File { get; set; }
    }
}
