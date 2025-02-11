using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ErettsegizzunkApi.DTOs
{
    public class BackupRestoreDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public IFormFile File { get; set; } // File Upload Property
    }
}
