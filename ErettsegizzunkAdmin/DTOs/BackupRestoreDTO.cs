using System.ComponentModel.DataAnnotations;

namespace ErettsegizzunkApi.DTOs
{
    public class BackupRestoreDTO
    {
        public string Token { get; set; }

        public string FileName { get; set; }
    }

    public class GetBackupFileNamesDTO
    {
        public bool IsSelected { get; set; }
        public string FileName { get; set; }
    }
}
