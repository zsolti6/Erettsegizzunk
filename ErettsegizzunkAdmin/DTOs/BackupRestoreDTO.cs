namespace ErettsegizzunkAdmin.DTOs
{
    public class BackupRestoreDTO : ParentDTO
    {
        public string FileName { get; set; }
    }

    public class GetBackupFileNamesDTO
    {
        public bool IsSelected { get; set; }
        public string FileName { get; set; }
    }
}
