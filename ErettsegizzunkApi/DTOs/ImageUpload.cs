namespace ErettsegizzunkApi.DTOs
{
    public class ImageUpload
    {
        public string Token { get; set; }
        
        public IFormFile File { get; set; }
    }
}
