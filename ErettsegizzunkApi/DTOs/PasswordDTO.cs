namespace ErettsegizzunkApi.DTOs
{
    public class PasswordChangeDTO : ParentDTO
    { 
        public string LoginName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
